using Common.Lib.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts;
using Service.DTOs;
using Service.Entities;
using Service.Helpers.Extensions;

namespace Service.Controllers;

[ApiController]
[Route("/api/items")]
public sealed class ItemsController : ControllerBase
{
    private readonly IRepository<Item> itemsRepository;
    private readonly IPublishEndpoint publishEndpoint;


    #region CTOR
    public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
    {
        this.itemsRepository = itemsRepository;
        this.publishEndpoint = publishEndpoint;
    }
    #endregion

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDTO>>> GetAllAsync()
    {
        var items = (await itemsRepository.GetAllAsync())
                    .Select(item => item.AsDto());

        if (!items.Any())
            return NoContent();
        else
            return Ok(items);
    }

    [HttpGet("{id:guid}", Name = "GetById")]
    public async Task<ActionResult<ItemDTO>> GetById(Guid id)
    {
        var item = await itemsRepository.GetAsync(id);

        return item == null ? NotFound() : Ok(item.AsDto());
    }

    [HttpPost]
    public async Task<ActionResult<CreateItemDTO>> Create(CreateItemDTO createItemDTO)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }



        var item = new Item
        {
            CreatedDate = DateTimeOffset.UtcNow,
            Description = createItemDTO.Description,
            Price = createItemDTO.Price,
            Name = createItemDTO.Name
        };

        await itemsRepository.CreateAsync(item);

        await publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));

        return CreatedAtAction("GetById", new { id = item.Id }, item);
    }

    [HttpPut("{id:guid:required}")]
    public async Task<IActionResult> Put(Guid id, UpdateItemDTO updateItemDTO)
    {
        var existingItem = await itemsRepository.GetAsync(id);

        if (existingItem is null)
            return NotFound();

        existingItem.Name = updateItemDTO.Name;
        existingItem.Description = updateItemDTO.Description;
        existingItem.Price = updateItemDTO.Price;

        await itemsRepository.UpdateAsync(existingItem);

        await publishEndpoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description));


        return NoContent();
    }

    [HttpDelete("{id:guid:required}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existingItem = await itemsRepository.GetAsync(id);


        if (existingItem is null)
            return NotFound();

        await itemsRepository.RemoveAsync(id);
        await publishEndpoint.Publish(new CatalogItemDeleted(existingItem.Id));


        return NoContent();
    }




}
