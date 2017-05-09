namespace Morpher.WebApi.Services.Interfaces
{
    using System;

    using Morpher.WebApi.Models;

    public interface IMorpherDatabase
    {
        int GetDefaultDailyQueryLimit();

        int GetQueryCountByIp(string ip);

        int GetQueryCountByToken(Guid guid);

        MorpherCacheObject GetUserLimits(Guid guid);

        bool IsIpBlocked(string ip);
    }
}