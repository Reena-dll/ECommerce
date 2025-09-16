using ECommerce.Application.Core.Results;
using ECommerce.Application.DTOs.Category;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.Services.Abstracts;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.UnitOfWorks;
using Mapster;
using System.Linq.Expressions;


namespace ECommerce.Application.Services.Concretes;
public class CategoryService(IUnitOfWork unitOfWork, IHelperService helperService) : ICategoryService
{
    public async Task<Result> AddAsync(AddCategoryDto addCategoryDto)
    {
        var category = addCategoryDto.Adapt<Category>();
        var categoryRepo = unitOfWork.GetRepository<Category>();
        var checkSamePermisssion = categoryRepo.GetAllNoPaginationAsync(predicate: p => p.Name.Trim() == addCategoryDto.Name.Trim(), enableTracking: true);
        if (checkSamePermisssion.Result.Count() > 0) return Error.HasSameProduct;
        await categoryRepo.AddAsync(category);
        await unitOfWork.SaveAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(DeleteDto deleteDto)
    {
        var categoryRepo = unitOfWork.GetRepository<Category>();
        var productRepo = unitOfWork.GetRepository<Product>();

        var category = await categoryRepo.GetAsync(p => p.Id == deleteDto.Id, enableTracking: true);
        if (category == null) return Error.ProductNotExist;

        bool hasActiveProducts = await productRepo.AnyAsync(x => x.CategoryId == deleteDto.Id && !x.IsDeleted);
        if (hasActiveProducts)
            return Error.CategoryHasProducts;

        category.IsDeleted = true;
        await unitOfWork.SaveAsync();
        return Result.Success();
    }

    public async Task<Result<List<CategoryDto>>> GetAllAsync(GetCategoryDto getCategoryDto)
    {
        var categoryRepo = unitOfWork.GetRepository<Category>();

        var searchName = getCategoryDto.Name?.ToLower();

        Expression<Func<Category, bool>> filter = x =>
         (string.IsNullOrEmpty(searchName) || x.Name.ToLower().Contains(searchName));

        var categories = await categoryRepo.GetAllAsync(
            predicate: filter,
            select: s => new CategoryDto()
            {
                Id = s.Id,
                Name = s.Name,
                CreateDate = s.CreateDate,
                UpdateDate = s.UpdateDate,
                CreatedById = s.CreatedBy,
                UpdatedById = s.UpdatedBy,

            },
            orderBy: o => o.OrderByDescending(p => p.CreateDate),
            pageSize: getCategoryDto.PageSize,
            currentPage: getCategoryDto.PageNumber,
            enableTracking: false
        );

        await helperService.FillAudit(categories);

        var userCount = await categoryRepo.CountAsync(filter);

        return Result<List<CategoryDto>>.Success(categories, userCount);
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(Guid Id)
    {
        var categoryRepo = unitOfWork.GetRepository<Category>();
        var category = await categoryRepo.GetAsync(
            select: p => new CategoryDto
            {
                Id = p.Id,
                Name = p.Name,
                CreatedById = p.CreatedBy,
                UpdateDate = p.UpdateDate,
                UpdatedById = p.UpdatedBy,
            },
            predicate: p => p.Id == Id,
            enableTracking: false);

        await helperService.FillAudit(category);

        return Result<CategoryDto>.Success(category);
    }

    public async Task<Result> UpdateAsync(UpdateCategoryDto updateCategoryDto)
    {
        var categoryRepo = unitOfWork.GetRepository<Category>();
        var category = await categoryRepo.GetAsync(p => p.Id == updateCategoryDto.Id, enableTracking: true);
        if (category == null) return Error.ProductNotExist;
        category.Name = updateCategoryDto.Name;
        await unitOfWork.SaveAsync();
        return Result.Success();
    }
}
