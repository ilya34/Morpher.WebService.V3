namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;

    using Morpher.WebService.V3.Models;

    public interface IMorpherCache
    {
        bool Decrement(MorpherCacheObject morpherCacheObject);

        object Get(string key, string regionName = null);

        void Set(string key, object value, DateTimeOffset absoluteExpirationDateTimeOffset, string regionName = null);

        object Remove(string key, string regionName = null);
    }
}