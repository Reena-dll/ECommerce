using ECommerce.Application.DTOs.Authentication;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Abstracts;

public interface IAuthService
{
    JwtDto GenerateJwtToken(User user);
    string GetCurrentUserEmail();
}
