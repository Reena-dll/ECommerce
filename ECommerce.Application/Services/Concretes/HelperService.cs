using Microsoft.AspNetCore.Http;
using ECommerce.Application.Core.DTO;
using ECommerce.Application.Services.Abstracts;
using ECommerce.Infrastructure.UnitOfWorks;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Concretes;

public class HelperService(IUnitOfWork unitOfWork, IHttpContextAccessor _httpContextAccesor) : IHelperService
{
    public async Task FillAudit(IEnumerable<BaseDto> entities)
    {
        var userRepo = unitOfWork.GetRepository<User>();

        var allUserIds = entities
            .SelectMany(u => new[] { u.CreatedById, u.UpdatedById })
            .Where(id => id != null && id.HasValue)
            .Select(id => id!.Value)
            .Distinct()
            .ToList();

        var allUsers = await userRepo.GetAllNoPaginationAsync(
            select: s => new { s.Id, s.FullName },
            predicate: u => allUserIds.Contains(u.Id),
            enableTracking: false
        );

        var userLookup = allUsers.ToDictionary(u => u.Id, u => u.FullName);

        foreach (var entity in entities)
        {
            entity.CreatedBy = userLookup.TryGetValue(entity.CreatedById, out var createdByName) ? createdByName : string.Empty;
            entity.UpdatedBy = entity.UpdatedById.HasValue && userLookup.TryGetValue(entity.UpdatedById.Value, out var updatedByName) ? updatedByName : null;
        }
    }

    public async Task FillAudit(BaseDto entity)
    {
        if (entity is null) return;
        await FillAudit(new[] { entity });
    }

    public async Task<CustomUser> GetUserFromToken()
    {
        var customUser = _httpContextAccesor.HttpContext.Items["CustomUser"] as CustomUser;

        if (customUser != null)
        {
            var userId = customUser.UserId;
            var userEmail = customUser.UserEmail;
            var roles = customUser.Roles;
        }

        return customUser;
    }
}