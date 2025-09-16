using ECommerce.Application.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Category;

public class UpdateCategoryDto : BaseRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
