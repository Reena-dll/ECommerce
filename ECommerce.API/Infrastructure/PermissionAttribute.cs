using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ECommerce.Domain.Constants;
using System.Linq;
using System.Security.Claims;

namespace ECommerce.API.Infrastructure;

public class PermissionAttribute(params string[] permissions) : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!(user?.Identity?.IsAuthenticated ?? false))
        {
            context.Result = new ForbidResult();
            return;
        }

        var userPermissions = user.Claims
            .Where(c => c.Type == "Permissions")
            .SelectMany(c => c.Value.Split(','))
            .Select(p => p.Trim())
            .ToList();

        var hasClaim = permissions.Any(userPermissions.Contains);

        if (!hasClaim)
        {
            var userRoles = user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!userRoles.Contains(UserRoles.Creator.ToString()))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
