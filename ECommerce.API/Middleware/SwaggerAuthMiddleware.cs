using ECommerce.Application.Services.Abstracts;
using System.Text;

namespace ECommerce.API.Middleware;

public class SwaggerAuthMiddleware
{
    private readonly RequestDelegate _next;

    public SwaggerAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.Headers["WWW-Authenticate"] = "Basic";
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            try
            {
                var encoded = authHeader["Basic ".Length..].Trim();
                var bytes = Convert.FromBase64String(encoded);
                var decoded = Encoding.UTF8.GetString(bytes);
                var parts = decoded.Split(':');

                if (parts.Length != 2)
                    throw new FormatException("Invalid header");

                var username = parts[0];
                var password = parts[1];

                // 🔑 Kullanıcı doğrulaması
                var isValid = await userService.ValidateSwaggerCredentialsAsync(username, password);
                if (!isValid)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid credentials");
                    return;
                }
            }
            catch
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid Authorization header");
                return;
            }
        }

        await _next(context);
    }
}


