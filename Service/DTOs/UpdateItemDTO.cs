using System.ComponentModel.DataAnnotations;

namespace Service;

public record class UpdateItemDTO([Required] string Name,
                                  string Description,
                                  [Range(0,100) ]decimal Price);
