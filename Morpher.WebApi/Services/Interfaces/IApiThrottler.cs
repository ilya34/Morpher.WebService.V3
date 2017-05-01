namespace Morpher.WebApi.Services.Interfaces
{
    using System;
    using System.Net.Http;

    using Morpher.WebApi.ApiThrottler;
    using Morpher.WebApi.Models;

    public interface IApiThrottler
    {
        ApiThrottlingResult Throttle(string ip);

        ApiThrottlingResult Throttle(Guid guid, out bool paidUser);

        ApiThrottlingResult Throttle(HttpRequestMessage httpRequest, out bool paidUser);

        object RemoveFromCache(string key);

        CacheObject GetQueryLimit(string ip);

        CacheObject GetQueryLimit(Guid guid);
    }
}
