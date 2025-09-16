using ECommerce.Application.Core.Results;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Services.Abstracts;
using ECommerce.Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Concretes;

public class ProductService(IUnitOfWork unitOfWork, IHelperService helperService) : IProductService
{
    public Task<Result> AddAsync(AddProductDto addProductDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(DeleteDto deleteDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<ProductDto>>> GetAllAsync(GetProductDto getProductDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ProductDto>> GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(UpdateProductDto updateProductDto)
    {
        throw new NotImplementedException();
    }
}
