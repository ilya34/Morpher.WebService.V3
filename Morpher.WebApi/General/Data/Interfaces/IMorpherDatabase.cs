namespace Morpher.WebService.V3.General.Data
{
    using System;

    public interface IMorpherDatabase
    {
        int GetDefaultDailyQueryLimit();

        int GetQueryCountByIp(string ip);

        int GetQueryCountByToken(Guid guid);

        MorpherCacheObject GetUserLimits(Guid guid);

        bool IsIpBlocked(string ip);
    }
}