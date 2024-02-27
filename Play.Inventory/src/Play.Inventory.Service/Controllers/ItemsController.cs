
using Common.Lib.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.DTOs;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Helpers;

namespace Play.Inventory.Service.Controllers;


[ApiController]
[Route("api/items")]
public class ItemsController : ControllerBase
{
    private readonly IRepository<InventoryItem> itemsRepository;
    private readonly CatalogClient catalogClient;

    public ItemsController(IRepository<InventoryItem> itemsRepository, CatalogClient catalogClient)
    {
        this.itemsRepository = itemsRepository;
        this.catalogClient = catalogClient;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetUserItems(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("GUID is empty");
        }

        var catalogItems = await catalogClient.GetCatalogItemsAsync();
        var inventoryItemEntities = await itemsRepository.GetAllAsync( item => item.UserId == userId);

        var inventoryItemDtos = inventoryItemEntities.Select(inventoryItem =>
        {
            var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
            return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
        });

        if (!inventoryItemDtos.Any())
            return NoContent();
        else
            return Ok(inventoryItemDtos);
    }

    [HttpPost]
    public async Task<ActionResult> UpsertItem(GrantItemsDto grantItemsDto)
    {
        try
        {
            var inventoryItem = await itemsRepository
              .GetAsync(item => item.UserId == grantItemsDto.UserId && item.CatalogItemId == grantItemsDto.CatalogItemId);

            if (inventoryItem is null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    AcquiredData = DateTimeOffset.UtcNow,
                    Quantity = grantItemsDto.Quantity,
                    UserId = grantItemsDto.UserId
                };
                await itemsRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemsDto.Quantity;
                await itemsRepository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }



}


