
using Play.Inventory.Service.DTOs;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Helpers;

public static class Extensions
{
  public static InventoryItemDto AsDto(this InventoryItem item)
  {
    return new InventoryItemDto(item.CatalogItemId, item.Quantity, item.AcquiredData);
  }
}


