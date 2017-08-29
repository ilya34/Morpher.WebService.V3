namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Runtime.Caching;

    public interface IMorpherCache
    {
        object Get(string key, string regionName = null);

        void Set(string key, object value, DateTimeOffset absoluteExpirationDateTime, string regionName = null);

        void Set(string key, object value, CacheItemPolicy cacheItemPolicy, string regioName = null);

        object Remove(string key, string regionName = null);
    }
}