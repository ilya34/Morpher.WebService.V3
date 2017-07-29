// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Net.Http;

    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;

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

        public object RemoveFromCache(string key)
        {
            return new object();
        }

        public MorpherCacheObject GetQueryLimit(string ip)
        {
            return new MorpherCacheObject()
                       {
                           PaidUser = true,
                           Unlimited = true,
                           QueriesLeft = 1000
                       };
        }

        public MorpherCacheObject GetQueryLimit(Guid guid)
        {
            return new MorpherCacheObject()
                       {
                           PaidUser = true,
                           Unlimited = true,
                           QueriesLeft = 1000
                       };
        }
    }
}