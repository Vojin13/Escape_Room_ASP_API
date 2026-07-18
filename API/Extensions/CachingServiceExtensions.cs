using Application;
using ASPLAB2.API;
using Implementation.Caching;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace API.Extensions;

public static class CachingServiceExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services, AppSettings settings)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = settings.RedisSettings.ConnectionString;
            options.InstanceName = settings.RedisSettings.InstanceName;
        });

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}
