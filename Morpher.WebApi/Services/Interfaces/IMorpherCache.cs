namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;
    using System.Runtime.Caching;

    using Morpher.WebService.V3.Models;

    public interface IMorpherCache
    {
        bool Decrement(MorpherCacheObject morpherCacheObject);

        object Get(string key, string regionName = null);

        void Set(string key, object value, DateTimeOffset absoluteExpirationDateTime, string regionName = null);

        void Set(string key, object value, CacheItemPolicy cacheItemPolicy, string regioName = null);

        object Remove(string key, string regionName = null);
    }
}