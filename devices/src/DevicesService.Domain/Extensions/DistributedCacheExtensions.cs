using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace DevicesService.Domain.Extensions;

internal static class DistributedCacheExtensions
{
    extension(IDistributedCache cache)
    {
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            string serializedValue = JsonSerializer.Serialize(value);
            
            await cache.SetStringAsync(key, serializedValue, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
            });
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            var serializedValue = await cache.GetStringAsync(key);

            if (serializedValue is null) return null;
            
            return JsonSerializer.Deserialize<T>(serializedValue);
        }
    }
}