using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace ProfilesService.Infrastructure.Cache;

public static class DistributedCacheExtensions
{
    extension(IDistributedCache cache)
    {
        public async Task<T?> GetObjectAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var bytes = await cache.GetAsync(key, cancellationToken);

            if (bytes is null) 
                return default;
            
            return JsonSerializer.Deserialize<T>(bytes);
        }
        
        public async Task SetObjectAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
            await cache.SetAsync(key, bytes, cancellationToken);
        }
    }
}