using ECommerce.Application.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Category;

public class GetCategoryDto : BaseFilterDto
{
    public string? Name { get; set; }
}
