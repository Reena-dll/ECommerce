using Mapster;
using Microsoft.EntityFrameworkCore;
using ECommerce.Application.Core.Results;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.Role;
using ECommerce.Application.Services.Abstracts;
using ECommerce.Infrastructure.UnitOfWorks;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Concretes;

public class RoleService(IUnitOfWork unitOfWork, IHelperService helperService) : IRoleService
{
    public async Task<Result<List<RoleDto>>> GetAllRolesAsync()
    {
        var roleRepo = unitOfWork.GetRepository<Role>();
        var roles = await roleRepo.GetAllNoPaginationAsync(
            select: s => new RoleDto
            {
                Id = s.Id,
                RoleName = s.RoleName,
                CreateDate = s.CreateDate,
                CreatedById = s.CreatedBy,
                UpdateDate = s.UpdateDate,
                UpdatedById = s.UpdatedBy,

            },
        enableTracking: false);

        await helperService.FillAudit(roles);

        var roleCount = await roleRepo.CountAsync();

        return Result<List<RoleDto>>.Success(roles, roleCount);
    }
    public async Task<Result> AddRoleAsync(CreateRoleDto roleDto)
    {
        var roleRepo = unitOfWork.GetRepository<Role>();
        var checkSameRole = roleRepo.GetAllNoPaginationAsync(predicate: p => p.RoleName.Trim() == roleDto.RoleName.Trim(), enableTracking: true);
        if (checkSameRole.Result.Count() > 0) return Error.HasSameRole;
        var permissionRepo = unitOfWork.GetRepository<Permission>();
        var permissions = await permissionRepo.GetAllNoPaginationAsync(predicate: p => roleDto.PermissionIds.Contains(p.Id));
        var role = roleDto.Adapt<Role>();
        role.Permissions = permissions.ToList();
        await roleRepo.AddAsync(role);
        await unitOfWork.SaveAsync();
        return Result.Success();
    }
    public async Task<Result> UpdateRoleAsync(UpdateRoleDto roleDto)
    {
        var roleRepo = unitOfWork.GetRepository<Role>();
        var role = await roleRepo.GetAsync(r => r.Id == roleDto.Id, enableTracking: true);
        if (role == null) return Error.RoleNotExist;
        role.RoleName = roleDto.RoleName;
        await unitOfWork.SaveAsync();
        return Result.Success();
    }
    public async Task<Result<List<AllRolePermission>>> GetAllRolePermissionAsync(Guid? roleId = null)
    {
        var roleRepo = unitOfWork.GetRepository<Role>();

        var roles = await roleRepo.GetAllNoPaginationAsync(
            select: s => new AllRolePermission
            {
                RoleId = s.Id,
                RoleName = s.RoleName,
                RightList = s.Permissions.Where(p => !p.IsDeleted).Select(p => p.Name).ToList(),
                PermissionIds = s.Permissions.Where(p => !p.IsDeleted).Select(p => p.Id).ToList()
            },
            include: i => i.Include(p => p.Permissions.Where(x => !x.IsDeleted)),
            enableTracking: false);

        if (roleId.HasValue)
        {
            roles = roles.Where(r => r.RoleId == roleId.Value).ToList();
        }

        return Result<List<AllRolePermission>>.Success(roles);
    }
    public async Task<Result<AllRolePermission>> UpdateRolePermissionAsync(UpdateRolePermissionDto roleDto)
    {
        var roleRepo = unitOfWork.GetRepository<Role>();
        var permisionRepo = unitOfWork.GetRepository<Permission>();

        var role = await roleRepo.GetAsync(r => r.Id == roleDto.roleId, include: i => i.Include(r => r.Permissions),enableTracking:true);

        if (role == null)
            return Error.RoleNotExist;

        var newPermission = await permisionRepo.GetAllNoPaginationAsync(
            predicate: p => roleDto.PermissionIds.Contains(p.Id));

        if (newPermission == null || newPermission.Count == 0)
            return Error.PermissionNotExist;

        role.Permissions.Clear();
        foreach (var permission in newPermission)
        {
            role.Permissions.Add(permission);
        }

        await unitOfWork.SaveAsync();

        var updatedRole = new AllRolePermission
        {
            RoleId = role.Id,
            RoleName = role.RoleName,
            RightList = role.Permissions.Select(p => p.Name).ToList(),
            PermissionIds = role.Permissions.Where(p => !p.IsDeleted).Select(p => p.Id).ToList()
        };

        return Result<AllRolePermission>.Success(updatedRole);
    }
    public async Task<Result> DeleteRoleAsync(DeleteDto deleteDto)
    {
        var roleRepo = unitOfWork.GetRepository<Role>();
        var role = await roleRepo.GetAsync(r => r.Id == deleteDto.Id, enableTracking: true);
        if (role == null) return Error.RoleNotExist;
        role.IsDeleted = true;
        role.UpdateDate = DateTime.Now;
        await unitOfWork.SaveAsync();

        return Result.Success();
    }

}