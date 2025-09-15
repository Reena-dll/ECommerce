using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Infrastructure;
using ECommerce.Application.DTOs.Authentication;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.User;
using ECommerce.Application.Services.Abstracts;
using ECommerce.Application.Services.Concretes;

namespace ECommerce.API.Controllers;

public class UserController(IUserService userService) : BaseController
{
    [HttpGet("GetAll")]
    [Permission("DataDefinition.UserDefinition.View")]
    public async Task<IActionResult> GetAll([FromQuery] GetUserDto getUserDto)
    {
        var result = await userService.GetAllAsync(getUserDto);
        return Ok(result);
    }

    [HttpGet("GetById")]
    [Permission("DataDefinition.UserDefinition.View")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await userService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("TestLoginGetUser")]
    public async Task<IActionResult> TestLoginGetUser([FromQuery] GetUserDto getUserDto)
    {
        var result = await userService.GetAllAsync(getUserDto);
        return Ok(result);
    }

    [HttpPost("Add")]
    [Permission("DataDefinition.UserDefinition.Add")]
    public async Task<IActionResult> AddUser([FromBody] AddUserDto userDto)
    {
        var result = await userService.AddAsync(userDto);
        return Ok(result);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var result = await userService.LoginAsync(login);
        return Ok(result);
    }

    [HttpPost("TestLogin")]
    [AllowAnonymous]
    public async Task<IActionResult> TestLogin([FromBody] SsoLoginDto ssoLoginDto)
    {
        var result = await userService.TestLogin(ssoLoginDto);
        return Ok(result);
    }

    [HttpPost("Update")]
    [Permission("DataDefinition.UserDefinition.Edit")]

    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updatedUser)
    {
        var result = await userService.UpdateAsync(updatedUser);
        return Ok(result);
    }

    [HttpPost("Delete")]
    [Permission("DataDefinition.UserDefinition.Delete")]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteDto deletedUser)
    {
        var result = await userService.DeleteAsync(deletedUser);
        return Ok(result);
    }

    [HttpPost("ChangePassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        var result = await userService.ChangePassword(changePasswordDto);
        return Ok(result);
    }
    


}
