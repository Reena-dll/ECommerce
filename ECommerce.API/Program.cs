using ECommerce.API;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.SwaggerUI;
using ECommerce.Infrastructure;
using ECommerce.Application;
using ECommerce.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.LoadDataAccess(builder.Configuration);

builder.Services.LoadBusiness(builder.Configuration);

builder.Services.LoadWebApi();

builder.InitLogger();

var app = builder.Build();

app.UseExceptionHandler(_ => { });

app.UseMiddleware<SwaggerAuthMiddleware>();

app.UseMiddleware<ApiLoggingMiddleware>();

app.UseSwagger();

app.UseSwaggerUI(options =>
{

    options.SwaggerEndpoint($"/swagger/v1/swagger.json", "ECommerce API V1");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "ECommerce System API Documentation";
    options.DocExpansion(DocExpansion.None);
});


app.UseCors("AllowAllOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();