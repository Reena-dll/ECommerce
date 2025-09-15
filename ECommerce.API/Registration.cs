using Microsoft.OpenApi.Models;
using ECommerce.API.Infrastructure;

namespace ECommerce.API;

public static class Registration
{
    public static IServiceCollection LoadWebApi(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddControllers(opt =>
        {
            opt.Filters.Add<ActionInterceptor>();
        });
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicy("PermissionPolicy", policy => policy.RequireClaim("Permissions"));
        });
        serviceCollection.AddCors(options =>
        {
            options.AddPolicy(name: "AllowAllOrigins", builder =>
            {
                builder.WithOrigins("http://localhost:8044")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });
        serviceCollection.AddHttpContextAccessor();
        serviceCollection.AddHttpClient();
        serviceCollection.AddExceptionHandler<GlobalExceptionHandler>();
        return serviceCollection;
    }

    public static WebApplicationBuilder InitLogger(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        return builder;
    }
}
