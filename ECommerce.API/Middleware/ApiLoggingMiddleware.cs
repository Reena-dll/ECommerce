using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.IO;
using ECommerce.Application.Core.DTO;
using ECommerce.Infrastructure.Context;
using ECommerce.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.API.Middleware;
public class ApiLoggingMiddleware
{
    private readonly RequestDelegate next;
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

    public ApiLoggingMiddleware(RequestDelegate Next)
    {
        next = Next;
        _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
    }
    private static string ReadStreamInChunks(Stream stream)
    {
        const int readChunkBufferLength = 4096;

        stream.Seek(0, SeekOrigin.Begin);

        using var textWriter = new StringWriter();
        using var reader = new StreamReader(stream);

        var readChunk = new char[readChunkBufferLength];
        int readChunkLength;

        do
        {
            readChunkLength = reader.ReadBlock(readChunk,
                                               0,
                                               readChunkBufferLength);
            textWriter.Write(readChunk, 0, readChunkLength);

        } while (readChunkLength > 0);

        return textWriter.ToString();
    }
    private async Task<String> getRequestBody(HttpContext context)
    {
        context.Request.EnableBuffering();

        await using var requestStream = _recyclableMemoryStreamManager.GetStream();
        await context.Request.Body.CopyToAsync(requestStream);

        String reqBody = ReadStreamInChunks(requestStream);

        context.Request.Body.Position = 0;

        return reqBody;
    }
    private IEnumerable<Claim> GetTokenClaims(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        var handler = new JwtSecurityTokenHandler();

        if (token.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            return null;

        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            token = token.Substring("Bearer ".Length).Trim();

        try
        {
            var jsonToken = handler.ReadJwtToken(token);
            return jsonToken?.Claims;
        }
        catch (Exception)
        {
            return null;
        }
    }
    private string GetTokenInfo(string token)
    {
        var claims = GetTokenClaims(token);

        if (claims == null)
            return "InvalidToken";

        var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var userEmail = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var roles = string.Join(",", claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value));
        var agencyId = claims.FirstOrDefault(c => c.Type == "AgencyId")?.Value;
        var agencyName = claims.FirstOrDefault(c => c.Type == "AgencyName")?.Value;

        return $"UserID: {userId}, Email: {userEmail}, Roles: {roles}, AgencyName: {agencyName}, AgencyId: {agencyId}";
    }
    private CustomUser GetCustomUserFromToken(string token)
    {
        var claims = GetTokenClaims(token);

        if (claims == null)
            return null;

        var companiesClaim = claims.FirstOrDefault(c => c.Type == "Companies")?.Value;

        List<Guid>? companies = companiesClaim != null
            ? companiesClaim.Split(',')
                .Select(c => Guid.TryParse(c, out var guid) ? guid : (Guid?)null)
                .Where(guid => guid.HasValue)
                .Select(guid => guid.Value)
                .ToList()
            : null;

        var user = new CustomUser
        {
            UserId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
            UserEmail = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            Roles = string.Join(",", claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value)),
        };

        return user;
    }
    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext.Request.Path.StartsWithSegments("/swagger"))
        {
            await next.Invoke(httpContext);
            return;
        }

        var requestText = string.Empty;
        var startTime = DateTime.UtcNow;

        if (httpContext.Request.Method == HttpMethod.Get.Method)
        {
            var requestUrl = httpContext.Request.GetDisplayUrl();

            var queryString = httpContext.Request.QueryString.HasValue
                ? httpContext.Request.QueryString.ToString()
                : string.Empty;

            if (!string.IsNullOrEmpty(queryString))
                requestText = Uri.UnescapeDataString(queryString);

        }
        else
            requestText = await getRequestBody(httpContext);


        var originalBodyStream = httpContext.Response.Body;

        await using var responseBody = _recyclableMemoryStreamManager.GetStream();
        httpContext.Response.Body = responseBody;
        var responseTimeInSeconds = (DateTime.UtcNow - startTime).TotalSeconds;

        var authTokenInfo = httpContext.Request.Headers["Authorization"].ToString();
        var authToken = string.IsNullOrEmpty(authTokenInfo) ? "Guest" : GetTokenInfo(authTokenInfo);

        if (!string.IsNullOrEmpty(authTokenInfo))
        {
            var customUser = GetCustomUserFromToken(authTokenInfo);
            httpContext.Items["CustomUser"] = customUser;
        }

        await next.Invoke(httpContext);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(httpContext.Response.Body, Encoding.UTF8).ReadToEndAsync();
        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);



        var userIp = httpContext.Request.Headers["X-Forwarded-For"].ToString();
        if (string.IsNullOrEmpty(userIp))
            userIp = httpContext.Connection.RemoteIpAddress?.ToString();

        using (var scope = httpContext.RequestServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ECommerceDBContext>();

            var apiLog = new ApiLog
            {
                RequestMethod = httpContext.Request.Method,
                RequestUrl = httpContext.Request.Path,
                StatusCode = httpContext.Response.StatusCode,
                RequestBody = requestText,
                UserIp = userIp,
                UserAgent = httpContext.Request.Headers["User-Agent"].ToString(),
                ResponseBody = responseText,
                AuthTokenInfo = authToken,
                RequestTime = DateTime.UtcNow,
                ResponseTimeInSeconds = (decimal)responseTimeInSeconds
            };

            await dbContext.ApiLogs.AddAsync(apiLog);
            await dbContext.SaveChangesAsync();
        }

        await responseBody.CopyToAsync(originalBodyStream);
    }
}
