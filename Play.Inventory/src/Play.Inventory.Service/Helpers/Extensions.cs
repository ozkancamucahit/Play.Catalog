
using Play.Inventory.Service.DTOs;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Helpers;

public static class Extensions
{
  public static InventoryItemDto AsDto(this InventoryItem item, string name, string description)
  {
    return new InventoryItemDto(item.CatalogItemId, item.Quantity, name, description, item.AcquiredData);
  }
}


