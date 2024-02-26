

using System.ComponentModel.DataAnnotations;

namespace Service.DTOs;

public record CreateItemDTO ([Required] string Name,
                             string Description,
                            [Range(0,100)] decimal Price);