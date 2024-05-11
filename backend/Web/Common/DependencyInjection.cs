using Web.Common.Domain.Contexts;
using Web.Common.Domain.Configurations;

using Microsoft.EntityFrameworkCore;

namespace Web.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationConfiguration>(
            configuration.GetSection("AuthenticationConfiguration"));

        // services.Configure<TransportConfiguration>(
        //     configuration.GetSection("TransportConfiguration"));

        services.AddDbContext<ApplicationDBContext>(option =>
            option.UseSqlite(connectionString: configuration.GetConnectionString("SqliteConnection")));

        services.AddCors(
            option => option.AddDefaultPolicy(
                builder => builder
                    .AllowAnyMethod().AllowAnyHeader()
                    .AllowCredentials().SetIsOriginAllowed(origin => true)));
        return services;
    }
}
