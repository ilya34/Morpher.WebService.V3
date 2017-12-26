using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Morpher.WebService.V3.General.DummyServices
{
    public class MorpherCache : IMorpherCache
    {
        public string Name { get; }
        public bool FirstLoad { get; set; }

        public object Get(string key, string regionName = null) => new MorpherCacheObject();
        public void Set(string key, object value, DateTimeOffset absoluteExpirationDateTime, string regionName = null) { return; }
        public void Set(string key, object value, CacheItemPolicy cacheItemPolicy, string regioName = null) { return; }
        public long GetCount(string regionName = null) => 0;
        public List<KeyValuePair<string, MorpherCacheObject>> GetAll() => new List<KeyValuePair<string, MorpherCacheObject>>();
        public object Remove(string key, string regionName = null) => new MorpherCacheObject();
        public string GetAllAsJson() => string.Empty;
    }
}