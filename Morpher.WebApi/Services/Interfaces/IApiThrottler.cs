namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;
    using System.Net.Http;
    using Microsoft.Owin;
    using Morpher.WebService.V3.Models;

    public interface IApiThrottler
    {
        ApiThrottlingResult Throttle(string ip);

        ApiThrottlingResult Throttle(Guid guid, out bool paidUser);

        ApiThrottlingResult Throttle(IOwinRequest request);

        ApiThrottlingResult Throttle(HttpRequestMessage httpRequest, out bool paidUser);

        object RemoveFromCache(string key);

        MorpherCacheObject GetQueryLimit(string ip);

        MorpherCacheObject GetQueryLimit(Guid guid);
    }
}
