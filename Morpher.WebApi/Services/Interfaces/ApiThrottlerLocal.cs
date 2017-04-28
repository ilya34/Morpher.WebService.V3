namespace Morpher.WebApi.Services.Interfaces
{
    using System;
    using System.Net.Http;

    using Morpher.WebApi.ApiThrottler;
    using Morpher.WebApi.Models;

    public class ApiThrottlerLocal : IApiThrottler
    {
        public ApiThrottlingResult Throttle(string ip)
        {
            return ApiThrottlingResult.Success;
        }

        public ApiThrottlingResult Throttle(Guid guid, out bool paidUser)
        {
            paidUser = true;
            return ApiThrottlingResult.Success;
        }

        public ApiThrottlingResult Throttle(HttpRequestMessage httpRequest, out bool paidUser)
        {
            paidUser = true;
            return ApiThrottlingResult.Success;
        }

        public bool UpdateCache(string key)
        {
            return true;
        }

        public CacheObject GetQueryLimit(string ip)
        {
            return new CacheObject()
                       {
                           PaidUser = true,
                           Unlimited = true,
                           DailyLimit = 1000
                       };
        }

        public CacheObject GetQueryLimit(Guid guid)
        {
            return new CacheObject()
                       {
                           PaidUser = true,
                           Unlimited = true,
                           DailyLimit = 1000
                       };
        }
    }
}