namespace Service.DTOs;

public record ItemDTO (Guid Id,
                       string Name,
                       string Description,
                       decimal Price,
                       DateTimeOffset CreatedDate);


