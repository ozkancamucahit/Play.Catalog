
namespace Play.Inventory.Service.DTOs;


public record GrantItemsDto(Guid UserId,
                            Guid CatalogItemId,
                            int Quantity);
public record InventoryItemDto(Guid CatalogItemId,
                               int Quantity,
                               string Name,
                               string Description,
                               DateTimeOffset AcquiredDate);

public record CatalogItemDTO(Guid Id,
                             string Name,
                             string Description);

