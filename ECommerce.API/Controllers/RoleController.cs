using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Infrastructure;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.Role;
using ECommerce.Application.Services.Abstracts;

namespace ECommerce.API.Controllers;

public class RoleController(IRoleService roleService) : BaseController
{
    [HttpGet("GetAll")]
    [Permission("DataDefinition.RoleRightManagement.View", "DataDefinition.UserDefinition.View")]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await roleService.GetAllRolesAsync();
        return Ok(result);
    }

    [HttpPost("Add")]
    [Permission("DataDefinition.RoleRightManagement.Add")]
    public async Task<IActionResult> AddRole([FromBody] CreateRoleDto roleDto)
    {
        var result = await roleService.AddRoleAsync(roleDto);
        return Ok(result);
    }

    [HttpPost("Update")]
    [Permission("DataDefinition.RoleRightManagement.Edit")]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto roleDto)
    {
        var result = await roleService.UpdateRoleAsync(roleDto);
        return Ok(result);
    }

    [HttpPost("Delete")]
    [Permission("DataDefinition.RoleRightManagement.Delete")]
    public async Task<IActionResult> DeleteRole([FromBody] DeleteDto deleteDto)
    {
        var result = await roleService.DeleteRoleAsync(deleteDto);
        return Ok(result);
    }

    [HttpGet("GetAllRolePermissionAsync")]
    [Permission("DataDefinition.RoleRightManagement.View")]
    public async Task<IActionResult> GetAllRolePermissionAsync(Guid? roleId = null)
    {
        var result = await roleService.GetAllRolePermissionAsync(roleId);
        return Ok(result);
    }

    [HttpPost("UpdateRolePermissionAsync")]
    [Permission("DataDefinition.RoleRightManagement.Edit")]
    public async Task<IActionResult> UpdateRolePermissionAsync([FromBody] UpdateRolePermissionDto roleDto)
    {
        var result = await roleService.UpdateRolePermissionAsync(roleDto);
        return Ok(result);
    }
}

