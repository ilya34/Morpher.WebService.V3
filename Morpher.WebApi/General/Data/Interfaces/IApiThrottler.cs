namespace Morpher.WebService.V3.General.Data
{
    using System;
    using Microsoft.Owin;

    public interface IApiThrottler
    {
        ApiThrottlingResult Throttle(IOwinRequest request);

        object RemoveFromCache(string key);

        MorpherCacheObject GetQueryLimit(string ip);

        MorpherCacheObject GetQueryLimit(Guid guid);
    }
}
