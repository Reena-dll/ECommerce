using ECommerce.Application.Core.Results;
using ECommerce.Application.DTOs.Category;
using ECommerce.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Abstracts;

public interface ICategoryService
{
    Task<Result<List<CategoryDto>>> GetAllAsync(GetCategoryDto getCategoryDto);
    Task<Result<CategoryDto>> GetByIdAsync(Guid Id);
    Task<Result> AddAsync(AddCategoryDto addCategoryDto);
    Task<Result> UpdateAsync(UpdateCategoryDto updateCategoryDto);
    Task<Result> DeleteAsync(DeleteDto deleteDto);
}
