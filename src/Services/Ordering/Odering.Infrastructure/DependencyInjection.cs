using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;

namespace Odering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            ////Add services to the container.
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityIntercepror>();
            services.AddScoped<ISaveChangesInterceptor, DIspatchDomainEventInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString);
            }  );

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            return services;
        }
    }
}
