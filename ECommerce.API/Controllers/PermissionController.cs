using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Infrastructure;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.Permission;
using ECommerce.Application.Services.Abstracts;

namespace ECommerce.API.Controllers;

public class PermissionController(IPermissionService _permissionService) : BaseController
{
    [HttpGet("GetAll")]
    [Permission("DataDefinition.RoleRightManagement.View")]
    public async Task<IActionResult> GetAll([FromQuery] GetPermissionDto getPermissionDto)
    {
        var result = await _permissionService.GetAllAsync(getPermissionDto);
        return Ok(result);
    }

    [HttpGet("GetById")]
    [Permission("DataDefinition.RoleRightManagement.View")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _permissionService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost("Add")]
    [Permission("DataDefinition.RoleRightManagement.Add")]
    public async Task<IActionResult> Add([FromBody] AddPermissionDto permissionDto)
    {
        var result = await _permissionService.AddAsync(permissionDto);
        return Ok(result);
    }

    [HttpPost("Update")]
    [Permission("DataDefinition.RoleRightManagement.Edit")]
    public async Task<IActionResult> Update([FromBody] UpdatePermissionDto permissionDto)
    {
        var result = await _permissionService.UpdateAsync(permissionDto);
        return Ok(result);
    }

    [HttpPost("Delete")]
    [Permission("DataDefinition.RoleRightManagement.Delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteDto deleteDto)
    {
        var result = await _permissionService.DeleteAsync(deleteDto);
        return Ok(result);
    }
}

