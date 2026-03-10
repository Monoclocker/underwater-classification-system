using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProfilesService.Infrastructure.Cache;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddRedisDistributedCache(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Redis")
                                      ?? throw new ArgumentException("Redis connection string is required");
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = "ProfilesService";
            });
        }
    }
}