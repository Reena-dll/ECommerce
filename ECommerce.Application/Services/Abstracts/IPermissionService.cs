using ECommerce.Application.Core.Results;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.Permission;

namespace ECommerce.Application.Services.Abstracts;

public interface IPermissionService
{
    Task<Result<List<PermissionDto>>> GetAllAsync(GetPermissionDto getPermissionDto);
    Task<Result<PermissionDto>> GetByIdAsync(Guid Id);
    Task<Result> AddAsync(AddPermissionDto permissionDto);
    Task<Result> UpdateAsync(UpdatePermissionDto permissionDto);
    Task<Result> DeleteAsync(DeleteDto deleteDto);
}
