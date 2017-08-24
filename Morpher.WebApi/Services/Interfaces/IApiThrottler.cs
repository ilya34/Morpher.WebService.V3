namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;
    using Microsoft.Owin;
    using V3.Models;

    public interface IApiThrottler
    {
        ApiThrottlingResult Throttle(IOwinRequest request);

        object RemoveFromCache(string key);

        MorpherCacheObject GetQueryLimit(string ip);

        MorpherCacheObject GetQueryLimit(Guid guid);
    }
}
