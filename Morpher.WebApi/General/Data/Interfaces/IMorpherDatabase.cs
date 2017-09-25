namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Collections.Generic;
    using Services;

    public interface IMorpherDatabase
    {
        int GetDefaultDailyQueryLimit();

        List<KeyValuePair<string, MorpherCacheObject>> GetMorpherCache();

        void UploadMorpherCache(List<KeyValuePair<string, MorpherCacheObject>> cache);

        MorpherCacheObject GetUserLimits(Guid guid);

        bool IsIpBlocked(string ip);
    }
}