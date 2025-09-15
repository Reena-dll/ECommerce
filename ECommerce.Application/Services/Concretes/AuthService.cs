using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce.Application.DTOs.Authentication;
using ECommerce.Application.Services.Abstracts;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Concretes;

public class AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IAuthService
{
    public JwtDto GenerateJwtToken(User user)
    {
        var secretKey = Encoding.ASCII.GetBytes(configuration["JWT:SecretKey"]!.ToString());

        var issuerSigningKey = new SymmetricSecurityKey(secretKey);

        var signInCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);

        var permissions = user.Roles
           .SelectMany(r => r.Permissions)
           .Select(p => p.Name)
           .Distinct()
           .ToList();

        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim("Permissions", string.Join(",", permissions)),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        authClaims.AddRange(
            user.Roles.Select(r => new Claim(ClaimTypes.Role, r.RoleName))
        );
       

        DateTime ExpireTime = DateTime.Now.AddMinutes(120);

        var token = new JwtSecurityToken(
            expires: ExpireTime,
            claims: authClaims,
            signingCredentials: signInCredentials
        );

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new JwtDto() { ExpireTime = ExpireTime, Token = jwtToken };
    }

    public string GetCurrentUserEmail()
    {
        var userEmail = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(userEmail)) throw new ArgumentNullException(nameof(userEmail));
        return userEmail;
    }
}
