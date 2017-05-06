namespace Morpher.WebApi.Services.Interfaces
{
    using System;

    using Morpher.WebApi.Models;

    public interface IMorpherCache
    {
        bool Decrement(CacheObject cacheObject);

        object Get(string key, string regionName = null);

        void Set(string key, object value, DateTimeOffset absoluteExpirationDateTimeOffset, string regionName = null);

        object Remove(string key, string regionName = null);
    }
}