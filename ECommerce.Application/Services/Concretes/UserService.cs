using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ECommerce.Application.Core.Helper;
using ECommerce.Application.Core.Results;
using ECommerce.Application.DTOs.Authentication;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.User;
using ECommerce.Application.Services.Abstracts;

using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Infrastructure.UnitOfWorks;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Constants;

namespace ECommerce.Application.Services.Concretes
{
    public class UserService(IUnitOfWork unitOfWork, IConfiguration configuration, IAuthService authService, IAuthService oAuthService, IHelperService helperService) : IUserService
    {
        public async Task<Result> AddAsync(AddUserDto userDto)
        {
            var userRepo = unitOfWork.GetRepository<User>();
            var roleRepo = unitOfWork.GetRepository<Role>();


            var roles = await roleRepo.GetAllNoPaginationAsync(predicate: p => userDto.RoleId.Contains(p.Id));
            if (roles == null || roles.Count == 0) return Error.RoleNotExist;

            var sameUserCheck = await userRepo.AnyAsync(p => p.PersonnelNumber.ToLower() == userDto.PersonnelNumber.ToLower() || p.Username == userDto.Username || p.Email.Trim().ToLower() == userDto.Email.Trim().ToLower());
            if (sameUserCheck) return Error.UserExist;

            var user = userDto.Adapt<User>();
            user.Roles = roles.DistinctBy(x => x.Id).ToList();

            if (!String.IsNullOrEmpty(userDto.Password))
            {
                var (hashedPassword, salt) = HashingHelper.HashPassword(userDto.Password);
                user.Password = hashedPassword;
                user.Salt = salt;
            }

            await userRepo.AddAsync(user);
            var result = await unitOfWork.SaveAsync();

            return Result.Success();
        }
        public async Task<Result> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var userRepo = unitOfWork.GetRepository<User>();

            var user = await userRepo.GetAsync(predicate: p => p.Email == changePasswordDto.Email && !p.IsDeleted);

            if (user == null || !string.IsNullOrEmpty(changePasswordDto.OldPassword) && !HashingHelper.VerifyPassword(changePasswordDto.OldPassword, user.Password, user.Salt))
            {
                return Error.UserNotFound;
            }

            if (!string.IsNullOrEmpty(changePasswordDto.NewPassword))
            {
                var (hashedPassword, salt) = HashingHelper.HashPassword(changePasswordDto.NewPassword);
                user.Password = hashedPassword;
                user.Salt = salt;
            }
            await unitOfWork.SaveAsync();

            return Result.Success();

        }
        public async Task<Result> DeleteAsync(DeleteDto deleteDto)
        {
            var userRepo = unitOfWork.GetRepository<User>();
            var user = await userRepo.GetAsync(u => u.Id == deleteDto.Id);
            if (user == null) return Error.UserNotExist;

            user.IsDeleted = true;

            await unitOfWork.SaveAsync();

            return Result.Success();
        }
        public async Task<Result<List<UserDto>>> GetAllAsync(GetUserDto getUserDto)
        {
            var userRepo = unitOfWork.GetRepository<User>();

            var searchName = getUserDto.Name?.ToLower();

            Expression<Func<User, bool>> filter = x =>
                string.IsNullOrEmpty(searchName) ||
                x.FirstName.ToLower().Contains(searchName) ||
                x.LastName.ToLower().Contains(searchName);

            var users = await userRepo.GetAllAsync(
                predicate: filter,
                select: s => new UserDto()
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Username = s.Username,
                    PersonnelNumber = s.PersonnelNumber,
                    CreateDate = s.CreateDate,
                    UpdateDate = s.UpdateDate,
                    CreatedById = s.CreatedBy,
                    UpdatedById = s.UpdatedBy,
                    RoleList = s.Roles.Select(x => new DTOs.Role.RoleForUser { Id = x.Id, RoleName = x.RoleName }).ToList(),
                },
                include: x => x.Include(x => x.Roles),
                orderBy: o => o.OrderByDescending(p => p.CreateDate),
                pageSize: getUserDto.PageSize,
                currentPage: getUserDto.PageNumber,
                enableTracking: false
            );

            await helperService.FillAudit(users);

            var userCount = await userRepo.CountAsync(filter);

            return Result<List<UserDto>>.Success(users, userCount);
        }
        public async Task<Result<UserDto>> GetByIdAsync(Guid Id)
        {
            var userRepo = unitOfWork.GetRepository<User>();

            var user = await userRepo.GetAsync(
                predicate: p => p.Id == Id,
                select: s => new UserDto()
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Username = s.Username,
                    PersonnelNumber = s.PersonnelNumber,
                    CreateDate = s.CreateDate,
                    UpdateDate = s.UpdateDate,
                    CreatedById = s.CreatedBy,
                    UpdatedById = s.UpdatedBy,
                    RoleList = s.Roles.Select(x => new DTOs.Role.RoleForUser { Id = x.Id, RoleName = x.RoleName }).ToList(),
                },
                include: x => x.Include(x => x.Roles),
                enableTracking: false);

            await helperService.FillAudit(user);

            return Result<UserDto>.Success(user);
        }
        public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto login)
        {
            var userRepo = unitOfWork.GetRepository<User>();
            var user = await userRepo.GetAsync(
                predicate: u => u.Email.Trim() == login.Email.Trim(),
                include: p => p.Include(x => x.Roles.Where(r => !r.IsDeleted)).ThenInclude(t => t.Permissions),
                enableTracking: false
            );

            if (user == null)
                return Error.UserNotExist;

            if (!string.IsNullOrEmpty(login.Password) && !HashingHelper.VerifyPassword(login.Password, user.Password, user.Salt))
                return Error.UserNotFound;


            var token = authService.GenerateJwtToken(user);

            var response = new LoginResponseDto()
            {
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.Username,
                Token = token.Token,
                ExpireTime = token.ExpireTime,
                RightList = user.Roles.SelectMany(p => p.Permissions).Select(p => p.Name).OrderByDescending(p => p).ToList(),
                Roles = user.Roles.Select(p => p.RoleName).ToList(),
            };

            return Result<LoginResponseDto>.Success(response);
        }
        public async Task<bool> ValidateSwaggerCredentialsAsync(string email, string password)
        {
            var userRepo = unitOfWork.GetRepository<User>();
            var user = await userRepo.GetAsync(
                predicate: u => u.Email.Trim() == email.Trim(),
                include: p => p.Include(x => x.Roles.Where(r => !r.IsDeleted)).ThenInclude(t => t.Permissions),
                enableTracking: false
            );

            if (user == null)
                return false;

            if (!string.IsNullOrEmpty(password) && !HashingHelper.VerifyPassword(password, user.Password, user.Salt))
                return false;

            if (!user.Roles.Any(x => x.RoleName == UserRoles.Creator.ToString()))
                return false;

            return true;
        }
       
        public async Task<Result> UpdateAsync(UpdateUserDto updatedUser)
        {
            var userRepo = unitOfWork.GetRepository<User>();
            var roleRepo = unitOfWork.GetRepository<Role>();

            var sameCode = await userRepo.AnyAsync(p => p.PersonnelNumber.ToLower() == updatedUser.PersonnelNumber.ToLower() && p.Id != updatedUser.Id);
            if (sameCode) return Error.UserExist;

            var isEmailInUse = await userRepo.AnyAsync(p => p.Email.Trim().ToLower() == updatedUser.Email.Trim().ToLower() && p.Id != updatedUser.Id);
            if (isEmailInUse) return Error.EmailAlreadyInUse;
           
            var user = await userRepo.GetAsync(
                predicate: p => p.Id == updatedUser.Id,
                include: i => i.Include(p => p.Roles),
                enableTracking: true);

            if (user == null) return Error.UserNotExist;

            // Kullanıcının sahip olması gereken roller
            var newRoles = await roleRepo.GetAllNoPaginationAsync(
                predicate: p => updatedUser.RoleId.Contains(p.Id));

            if (newRoles.Count != updatedUser.RoleId.Count)
                return Error.RoleNotExist;

            // Kullanıcının güncel rollerini ayarla
            user.Roles.Clear();
            foreach (var role in newRoles)
            {
                user.Roles.Add(role);
            }
            user.Roles.DistinctBy(p => p.Id);
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.PersonnelNumber = updatedUser.PersonnelNumber;


            await unitOfWork.SaveAsync();

            return Result.Success();
        }

    }
}
