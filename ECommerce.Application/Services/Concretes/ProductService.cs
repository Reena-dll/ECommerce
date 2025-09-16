using ECommerce.Application.Core.Results;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Services.Abstracts;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.UnitOfWorks;
using LinqKit;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.Application.Services.Concretes;

public class ProductService(IUnitOfWork unitOfWork, IHelperService helperService) : IProductService
{
    public async Task<Result> AddAsync(AddProductDto addProductDto)
    {
        var product = addProductDto.Adapt<Product>();
        var productRepo = unitOfWork.GetRepository<Product>();
        var checkSamePermisssion = productRepo.GetAllNoPaginationAsync(predicate: p => p.Name.Trim() == addProductDto.Name.Trim(), enableTracking: true);
        if (checkSamePermisssion.Result.Count() > 0) return Error.HasSameProduct;
        await productRepo.AddAsync(product);
        await unitOfWork.SaveAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(DeleteDto deleteDto)
    {
        var productRepo = unitOfWork.GetRepository<Product>();
        var product = await productRepo.GetAsync(p => p.Id == deleteDto.Id, enableTracking: true);
        if (product == null) return Error.ProductNotExist;

        product.IsDeleted = true;
        await unitOfWork.SaveAsync();
        return Result.Success();
    }

    public async Task<Result<List<ProductDto>>> GetAllAsync(GetProductDto getProductDto)
    {
        var productRepo = unitOfWork.GetRepository<Product>();

        var searchName = getProductDto.Name?.ToLower();

        Expression<Func<Product, bool>> filter = x =>
         (string.IsNullOrEmpty(searchName) || x.Name.ToLower().Contains(searchName));

        if (getProductDto.MinPrice.HasValue)
            filter = filter.And(x => x.Price >= getProductDto.MinPrice.Value);
        
        if (getProductDto.MaxPrice.HasValue)
            filter = filter.And(x => x.Price <= getProductDto.MaxPrice.Value);

        if (getProductDto.MinStock.HasValue)
            filter = filter.And(x => x.Stock >= getProductDto.MinStock.Value);
        
        if (getProductDto.MaxStock.HasValue)
            filter = filter.And(x => x.Stock <= getProductDto.MaxStock.Value);

        if (getProductDto.CategoryIds != null && getProductDto.CategoryIds.Any())
            filter = filter.And(x => getProductDto.CategoryIds.Contains(x.CategoryId));
        
        var users = await productRepo.GetAllAsync(
            predicate: filter,
            select: s => new ProductDto()
            {
                Id = s.Id,
                Name = s.Name,
                CategoryName = s.Category.Name,
                CategoryId = s.CategoryId,
                Price = s.Price,
                Stock = s.Stock,
                CreateDate = s.CreateDate,
                UpdateDate = s.UpdateDate,
                CreatedById = s.CreatedBy,
                UpdatedById = s.UpdatedBy,

            },
            include: x => x.Include(x => x.Category),
            orderBy: o => o.OrderByDescending(p => p.CreateDate),
            pageSize: getProductDto.PageSize,
            currentPage: getProductDto.PageNumber,
            enableTracking: false
        );

        await helperService.FillAudit(users);

        var userCount = await productRepo.CountAsync(filter);

        return Result<List<ProductDto>>.Success(users, userCount);
    }

    public async Task<Result<ProductDto>> GetByIdAsync(Guid Id)
    {
        var productRepo = unitOfWork.GetRepository<Product>();
        var product = await productRepo.GetAsync(
            select: p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                CategoryName = p.Category.Name,
                CategoryId = p.CategoryId,
                Price = p.Price,
                Stock = p.Stock,
                CreatedById = p.CreatedBy,
                UpdateDate = p.UpdateDate,
                UpdatedById = p.UpdatedBy,
            },
            predicate: p => p.Id == Id,
            include: x => x.Include(x => x.Category),
            enableTracking: false);

        await helperService.FillAudit(product);

        return Result<ProductDto>.Success(product);
    }

    public async Task<Result> UpdateAsync(UpdateProductDto updateProductDto)
    {
        var productRepo = unitOfWork.GetRepository<Product>();
        var product = await productRepo.GetAsync(p => p.Id == updateProductDto.Id, enableTracking: true);
        if (product == null) return Error.ProductNotExist;
        product.Name = updateProductDto.Name;
        product.Price = updateProductDto.Price;
        product.Stock = updateProductDto.Stock;
        product.CategoryId = updateProductDto.CategoryId;
        await unitOfWork.SaveAsync();
        return Result.Success();
    }
}
