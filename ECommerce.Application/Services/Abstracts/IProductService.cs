using ECommerce.Application.Core.Results;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.Product;

namespace ECommerce.Application.Services.Abstracts;

public interface IProductService
{
    Task<Result<List<ProductDto>>> GetAllAsync(GetProductDto getProductDto);
    Task<Result<ProductDto>> GetByIdAsync(Guid Id);
    Task<Result> AddAsync(AddProductDto addProductDto);
    Task<Result> UpdateAsync(UpdateProductDto updateProductDto);
    Task<Result> DeleteAsync(DeleteDto deleteDto);
}
