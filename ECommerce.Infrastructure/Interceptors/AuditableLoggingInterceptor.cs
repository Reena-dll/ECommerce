using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using ECommerce.Domain.Entities;
using System.Reflection;

namespace ECommerce.Infrastructure.Interceptors;

public class AuditableLoggingInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditableProperties(eventData.Context);
        LogChanges(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateAuditableProperties(eventData.Context);
        LogChanges(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void LogChanges(DbContext? dbContext)
    {
        if (dbContext == null)
            return;
        

        var logEntries = dbContext.ChangeTracker.Entries()
            .Where(p => p.Entity is BaseEntity && (p.State == EntityState.Added || p.State == EntityState.Modified || p.State == EntityState.Deleted))
            .Select(p => new Log()
            {
                EntityName = p.Entity.GetType().Name,
                Operation = p.State.ToString(),
                EntityId = ((BaseEntity)p.Entity).Id,
                BeforeState = p.State == EntityState.Added ? null : JsonSerializer.Serialize(p.OriginalValues.ToObject()),
                AfterState = p.State == EntityState.Deleted ? null : JsonSerializer.Serialize(p.CurrentValues.ToObject()),
            }).ToList();

        if (logEntries.Count != 0)
            dbContext.Set<Log>().AddRange(logEntries);
        
    }

    private void UpdateAuditableProperties(DbContext? dbContext)
    {
        if (dbContext == null)
            return;

        var userIdClaimValue = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var isParsed = Guid.TryParse(userIdClaimValue, out var userId);

        if (!isParsed) userId = Guid.Empty;

        foreach (var entry in dbContext.ChangeTracker.Entries().Where(p => p.Entity is BaseEntity))
        {
            if (entry.State == EntityState.Added)
            {
                ((BaseEntity)entry.Entity).CreatedBy = userId;
            }
            else if (entry.State == EntityState.Modified)
            {
                ((BaseEntity)entry.Entity).UpdatedBy = userId;
                ((BaseEntity)entry.Entity).UpdateDate = BaseEntity.GetTurkeyTime(DateTime.UtcNow);
            }
        }
    }
}
