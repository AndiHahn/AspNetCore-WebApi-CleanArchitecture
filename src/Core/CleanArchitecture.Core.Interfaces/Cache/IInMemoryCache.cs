using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Cache
{
    public interface IInMemoryCache<TItem>
    {
        Task<TItem> GetOrCreateAsync(object key, Func<Task<TItem>> createItemFunc);
        TItem Create(object key, TItem item, TimeSpan expiration);
        bool TryGetItem(object key, out TItem item);
        void RemoveItem(object key);
    }
}
