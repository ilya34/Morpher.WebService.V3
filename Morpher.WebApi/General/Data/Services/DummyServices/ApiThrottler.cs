using System;
using Microsoft.Owin;

namespace Morpher.WebService.V3.General.DummyServices
{
    public class ApiThrottler : IApiThrottler
    {
        public ApiThrottlingResult Throttle(IOwinRequest request, int count) => ApiThrottlingResult.Success;
        public ApiThrottlingResult LoadIntoCache(IOwinRequest request) => ApiThrottlingResult.Success;
        public object RemoveFromCache(string key) => null;
        public MorpherCacheObject GetQueryLimit(string ip) => new MorpherCacheObject() {PaidUser = true, QueriesLeft = 1000, Unlimited = true};
        public MorpherCacheObject GetQueryLimit(Guid guid) => new MorpherCacheObject() { PaidUser = true, QueriesLeft = 1000, Unlimited = true };
    }
}