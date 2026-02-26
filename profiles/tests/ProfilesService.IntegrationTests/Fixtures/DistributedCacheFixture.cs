using Microsoft.Extensions.Caching.StackExchangeRedis;
using Testcontainers.Redis;

namespace ProfilesService.IntegrationTests.Fixtures;

public sealed class DistributedCacheFixture : IAsyncLifetime
{
    private readonly RedisContainer _container = new RedisBuilder("redis:latest")
        .Build();

    public RedisCache GetDistributedCache()
    {
        var redisOptions = new RedisCacheOptions()
        {
            Configuration = _container.GetConnectionString(),
            InstanceName = "test"
        };
        
        return new RedisCache(redisOptions);
    }
    
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();
    }
}