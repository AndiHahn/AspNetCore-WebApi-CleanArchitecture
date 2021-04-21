using System;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.Application.Cache
{
    public class InMemoryCache<TItem> : IInMemoryCache<TItem>
    {
        private readonly MemoryCache cache;

        public InMemoryCache()
        {
            cache = new MemoryCache(
                new MemoryCacheOptions
                {
                    SizeLimit = 1024
                });
        }

        public TItem Create(object key, TItem item, TimeSpan expiration)
        {
            if (cache.TryGetValue(key, out TItem cacheEntry))
            {
                cache.Remove(key);
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1)
                .SetPriority(CacheItemPriority.High)
                .SetAbsoluteExpiration(expiration);

            cache.Set(key, item, cacheEntryOptions);

            return cacheEntry;
        }

        public async Task<TItem> GetOrCreateAsync(object key, Func<Task<TItem>> createItemFunc)
        {
            if (!cache.TryGetValue(key, out TItem cacheEntry))
            {
                cacheEntry = await createItemFunc();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSize(1)
                    .SetPriority(CacheItemPriority.High)
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

                cache.Set(key, cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;
        }

        public void RemoveItem(object key)
        {
            cache.Remove(key);
        }

        public bool TryGetItem(object key, out TItem item)
        {
            if (cache.TryGetValue(key, out item))
            {
                return true;
            }

            return false;
        }
    }
}
