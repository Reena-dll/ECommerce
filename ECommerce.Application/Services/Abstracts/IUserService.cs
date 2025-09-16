using ECommerce.Application.Core.Results;
using ECommerce.Application.DTOs.Authentication;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.Permission;
using ECommerce.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services.Abstracts
{
    public interface IUserService
    {
        Task<Result<List<UserDto>>> GetAllAsync(GetUserDto getUserDto);
        Task<Result<UserDto>> GetByIdAsync(Guid Id);
        Task<Result<LoginResponseDto>> LoginAsync(LoginDto login);
        Task<bool> ValidateSwaggerCredentialsAsync(string email, string password);
        Task<Result> AddAsync(AddUserDto user);
        Task<Result> UpdateAsync(UpdateUserDto updatedUser);
        Task<Result> DeleteAsync(DeleteDto deleteDto);
        Task<Result> ChangePassword(ChangePasswordDto changePasswordDto);
    }
}
