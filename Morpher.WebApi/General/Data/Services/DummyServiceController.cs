using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Morpher.WebService.V3.General.Data.Services
{
    using Microsoft.Owin;
    using System.Collections.Generic;
    using System.Runtime.Caching;
    public class DummyServiceController : IApiThrottler, IMorpherLog, IMorpherDatabase, IMorpherCache
    {
        // IApiThrottler
        public ApiThrottlingResult LoadIntoCache(IOwinRequest request) { return 0; }
        public ApiThrottlingResult Throttle(Guid guid, int cost) { return 0; }
        public ApiThrottlingResult Throttle(string ip, int cost) { return 0; }
        public ApiThrottlingResult Throttle(IOwinRequest request, int cost) { return 0; }
        public object RemoveFromCache(string key) { return null; }
        public MorpherCacheObject GetQueryLimit(string ip) { return null; }
        public MorpherCacheObject GetQueryLimit(Guid guid) { return null; }

        // IMorpherLog
        public void Log(IOwinContext context) { return; }
        public void Sync() { return; }

        // IMorpherDatabase
        public int GetDefaultDailyQueryLimit() { return 0; }
        public List<KeyValuePair<string, MorpherCacheObject>> GetMorpherCache() { return null; }
        public void UploadMorpherCache(List<KeyValuePair<string, MorpherCacheObject>> cache) { return; }
        public MorpherCacheObject GetUserLimits(Guid guid) { return null; }
        public bool IsIpBlocked(string ip) { return false; }

        // IMorpherCache
        public string Name { get; }
        public bool FirstLoad { get; set; }
        public object Get(string key, string regionName = null) { return null; }
        public void Set(string key, object value, DateTimeOffset absoluteExpirationDateTime, string regionName = null) { return; }
        public void Set(string key, object value, CacheItemPolicy cacheItemPolicy, string regioName = null) { return; }
        public long GetCount(string regionName = null) { return 0; }
        public List<KeyValuePair<string, MorpherCacheObject>> GetAll() { return null; }
        public object Remove(string key, string regionName = null) { return null; }
        public string GetAllAsJson() { return null; }
    }
}