namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    public interface IMorpherCache
    {
        string Name { get; }

        bool FirstLoad { get; set; }

        object Get(string key, string regionName = null);

        void Set(string key, object value, DateTimeOffset absoluteExpirationDateTime, string regionName = null);

        void Set(string key, object value, CacheItemPolicy cacheItemPolicy, string regioName = null);

        long GetCount(string regionName = null);

        List<KeyValuePair<string, MorpherCacheObject>> GetAll();

        object Remove(string key, string regionName = null);
    }
}