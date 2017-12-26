using System;
using System.Collections.Generic;

namespace Morpher.WebService.V3.General.Data.Services.DummyServices
{
    public class MorpherDatabase : IMorpherDatabase
    {
        public int GetDefaultDailyQueryLimit() => 1000;
        public List<KeyValuePair<string, MorpherCacheObject>> GetMorpherCache() => new List<KeyValuePair<string, MorpherCacheObject>>();
        public void UploadMorpherCache(List<KeyValuePair<string, MorpherCacheObject>> cache) { return; }
        public MorpherCacheObject GetUserLimits(Guid guid) => new MorpherCacheObject() { PaidUser = true, QueriesLeft = 1000, Unlimited = true };
        public bool IsIpBlocked(string ip) => true;
    }
}