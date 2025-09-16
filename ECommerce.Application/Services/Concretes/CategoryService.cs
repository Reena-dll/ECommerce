using ECommerce.Application.Core.Results;
using ECommerce.Application.DTOs.Category;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.Services.Abstracts;
using ECommerce.Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Concretes;
public class CategoryService(IUnitOfWork unitOfWork, IHelperService helperService) : ICategoryService
{
    public Task<Result> AddAsync(AddCategoryDto addCategoryDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(DeleteDto deleteDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<CategoryDto>>> GetAllAsync(GetCategoryDto getCategoryDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<CategoryDto>> GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(UpdateCategoryDto updateCategoryDto)
    {
        throw new NotImplementedException();
    }
}
