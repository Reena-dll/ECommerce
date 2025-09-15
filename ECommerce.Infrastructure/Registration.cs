using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Infrastructure.Context;
using ECommerce.Infrastructure.Interceptors;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.UnitOfWorks;

namespace ECommerce.Infrastructure;

public static class Registration
{
    public static IServiceCollection LoadDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<AuditableLoggingInterceptor>();
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        serviceCollection.AddDbContext<ECommerceDBContext>((sp, opt) =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DbConnection") ?? throw new ArgumentNullException("DbConnection"), p => p.MinBatchSize(1).MaxBatchSize(100_000).EnableRetryOnFailure())
                .AddInterceptors(sp.GetRequiredService<AuditableLoggingInterceptor>(), new DateFirstInterceptor());
        });
        return serviceCollection;
    }
}
