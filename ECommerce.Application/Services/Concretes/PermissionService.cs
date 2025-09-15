using Mapster;
using ECommerce.Application.Core.Results;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.Permission;
using ECommerce.Application.Services.Abstracts;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.UnitOfWorks;

namespace ECommerce.Application.Services.Concretes;

public class PermissionService(IUnitOfWork unitOfWork, IHelperService helperService) : IPermissionService
{
    public async Task<Result<List<PermissionDto>>> GetAllAsync(GetPermissionDto getPermissionDto)
    {
        var permissionRepo = unitOfWork.GetRepository<Permission>();

        var permissions = await permissionRepo.GetAllAsync(
            select: p => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreateDate = p.CreateDate,
                CreatedById = p.CreatedBy,
                UpdateDate = p.UpdateDate,
                UpdatedById = p.UpdatedBy,
            },
            orderBy: p => p.OrderBy(permission => permission.Name),
            pageSize: getPermissionDto.PageSize,
            currentPage: getPermissionDto.PageNumber,
            enableTracking: false);

        await helperService.FillAudit(permissions);

        var permissionCount = await permissionRepo.CountAsync();

        return Result<List<PermissionDto>>.Success(permissions, permissionCount);
    }

    public async Task<Result<PermissionDto>> GetByIdAsync(Guid Id)
    {
        var permissionRepo = unitOfWork.GetRepository<Permission>();
        var permission = await permissionRepo.GetAsync(
            select: p => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreateDate = p.CreateDate,
                CreatedById = p.CreatedBy,
                UpdateDate = p.UpdateDate,
                UpdatedById = p.UpdatedBy,
            },
            predicate: p => p.Id == Id,
            enableTracking: false);

        await helperService.FillAudit(permission);

        return Result<PermissionDto>.Success(permission);
    }

    public async Task<Result> AddAsync(AddPermissionDto permissionDto)
    {
        var permission = permissionDto.Adapt<Permission>();
        var permissionRepo = unitOfWork.GetRepository<Permission>();
        var checkSamePermisssion = permissionRepo.GetAllNoPaginationAsync(predicate: p => p.Name.Trim() == permissionDto.Name.Trim(), enableTracking: true);
        if (checkSamePermisssion.Result.Count() > 0) return Error.HasSamePermission;
        await permissionRepo.AddAsync(permission);
        await unitOfWork.SaveAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateAsync(UpdatePermissionDto permissionDto)
    {
        var permissionRepo = unitOfWork.GetRepository<Permission>();
        var permission = await permissionRepo.GetAsync(p => p.Id == permissionDto.Id, enableTracking: true);
        if (permission == null) return Error.PermissionNotExist;
        permission.Name = permissionDto.Name;
        permission.Description = permissionDto.Description;
        await unitOfWork.SaveAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(DeleteDto deleteDto)
    {
        var permissionRepo = unitOfWork.GetRepository<Permission>();

        var permission = await permissionRepo.GetAsync(p => p.Id == deleteDto.Id, enableTracking: true);

        if (permission == null) return Error.PermissionNotExist;

        permission.IsDeleted = true;

        await unitOfWork.SaveAsync();

        return Result.Success();
    }
}
