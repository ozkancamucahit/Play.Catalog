﻿using Service.DTOs;
using Service.Entities;

namespace Service.Helpers.Extensions;

public static class Extensions
{
  public static ItemDTO AsDto(this Item item)
  {
    return new ItemDTO(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
  }
}
