using ECommerce.Application.Core.Results;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.Role;

namespace ECommerce.Application.Services.Abstracts
{
    public interface IRoleService
    {

        Task<Result<List<RoleDto>>> GetAllRolesAsync();

        Task<Result> AddRoleAsync(CreateRoleDto roleDto);

        Task<Result> UpdateRoleAsync(UpdateRoleDto roleDto);

        Task<Result> DeleteRoleAsync(DeleteDto deleteDto);
        Task<Result<List<AllRolePermission>>> GetAllRolePermissionAsync(Guid? roleId = null);
        Task<Result<AllRolePermission>> UpdateRolePermissionAsync(UpdateRolePermissionDto roleDto);
    }
}
