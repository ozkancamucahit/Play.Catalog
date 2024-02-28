
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
    private readonly IRepository<InventoryItem> inventoryItemsRepository;
    private readonly IRepository<CatalogItem> catalogItemsRepository;

    public ItemsController(IRepository<InventoryItem> inventoryItemsRepository,
                           IRepository<CatalogItem> catalogItemsRepository)
    {
        this.inventoryItemsRepository = inventoryItemsRepository;
        this.catalogItemsRepository = catalogItemsRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetUserItems(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("GUID is empty");
        }

        var inventoryItemEntities = await inventoryItemsRepository.GetAllAsync( item => item.UserId == userId);
        var itemIds = inventoryItemEntities.Select(item => item.CatalogItemId);
        var catalogItemEntities = await catalogItemsRepository.GetAllAsync(item => itemIds.Contains(item.Id));

        var inventoryItemDtos = inventoryItemEntities.Select(inventoryItem =>
        {
            var catalogItem = catalogItemEntities.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
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
            var inventoryItem = await inventoryItemsRepository
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
                await inventoryItemsRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemsDto.Quantity;
                await inventoryItemsRepository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }



}


