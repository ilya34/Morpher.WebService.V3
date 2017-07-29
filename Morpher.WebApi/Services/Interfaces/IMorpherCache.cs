// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;
    using System.Runtime.Caching;

    using Morpher.WebService.V3.Models;

    public interface IMorpherCache
    {
        object Get(string key, string regionName = null);

        void Set(string key, object value, DateTimeOffset absoluteExpirationDateTime, string regionName = null);

        void Set(string key, object value, CacheItemPolicy cacheItemPolicy, string regioName = null);

        object Remove(string key, string regionName = null);
    }
}