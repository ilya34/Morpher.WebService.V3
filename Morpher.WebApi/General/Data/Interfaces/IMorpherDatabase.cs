namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Collections.Generic;
    using Services;

    public interface IMorpherDatabase
    {
        int GetDefaultDailyQueryLimit();

        List<KeyValuePair<string, object>> GetMorpherCache();

        void UploadMorpherCache(List<KeyValuePair<string, object>> cache);

        MorpherCacheObject GetUserLimits(Guid guid);

        bool IsIpBlocked(string ip);
    }
}