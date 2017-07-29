// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;
    using System.Net.Http;

    using Morpher.WebService.V3.Models;

    public interface IApiThrottler
    {
        ApiThrottlingResult Throttle(string ip);

        ApiThrottlingResult Throttle(Guid guid, out bool paidUser);

        ApiThrottlingResult Throttle(HttpRequestMessage httpRequest, out bool paidUser);

        object RemoveFromCache(string key);

        MorpherCacheObject GetQueryLimit(string ip);

        MorpherCacheObject GetQueryLimit(Guid guid);
    }
}
