using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using ECommerce.Infrastructure.Context;

namespace ECommerce.Application;

public static class Registration
{
    public static IServiceCollection LoadBusiness(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var secretKey = Encoding.ASCII.GetBytes(configuration["JWT:SecretKey"]!.ToString());
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes().Where(p => p is { Namespace: not null, MemberType: MemberTypes.TypeInfo }).ToList();
        var abstracts = types.Where(p => p is { IsInterface: true } && p.Namespace!.Contains("Services.Abstracts")).ToList();
        var concretes = types.Where(p => p is { IsClass: true, IsAbstract: false } && p.Namespace!.Contains("Services.Concretes")).ToList();
        foreach (var @abstract in abstracts)
        {
            var concrete = concretes.FirstOrDefault(p => p.GetInterfaces().Contains(@abstract)) ?? throw new Exception("Invalid Service Definition");
            serviceCollection.AddScoped(@abstract, concrete);
        }
        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
            };
        });
        serviceCollection.AddValidatorsFromAssembly(assembly);
        serviceCollection.AddHealthChecks()
                .AddDbContextCheck<ECommerceDBContext>("database")
                .AddCheck("self", () => HealthCheckResult.Healthy());

        return serviceCollection;
    }
}
