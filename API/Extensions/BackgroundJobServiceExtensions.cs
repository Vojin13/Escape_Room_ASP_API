using ASPLAB2.API;
using Hangfire;
using Hangfire.PostgreSql;

namespace API.Extensions;

public static class BackgroundJobServiceExtensions
{
    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, AppSettings settings)
    {
        services.AddHangfire(config => config
            .UsePostgreSqlStorage(options => options
                .UseNpgsqlConnection(settings.ConnectionStrings.DefaultConnection)));
        services.AddHangfireServer();

        return services;
    }
}
