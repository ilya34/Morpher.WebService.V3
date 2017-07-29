// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;

    using Morpher.WebService.V3.Models;

    public interface IMorpherDatabase
    {
        int GetDefaultDailyQueryLimit();

        int GetQueryCountByIp(string ip);

        int GetQueryCountByToken(Guid guid);

        MorpherCacheObject GetUserLimits(Guid guid);

        bool IsIpBlocked(string ip);
    }
}