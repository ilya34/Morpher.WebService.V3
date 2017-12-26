﻿namespace Morpher.WebService.V3.General
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using General;

    public class UserCacheLoaderMiddleware : OwinMiddleware
    {
        private readonly IApiThrottler _apiThrottler;
        private readonly IMorpherCache _cache;
        private readonly IMorpherDatabase _database;

        public UserCacheLoaderMiddleware(
            OwinMiddleware next,
            IApiThrottler apiThrottler,
            IMorpherCache cache,
            IMorpherDatabase database) : base(next)
        {
            _apiThrottler = apiThrottler;
            _cache = cache;
            _database = database;
        }

        public override Task Invoke(IOwinContext context)
        {
            if (_cache.FirstLoad)
            {
                _cache.FirstLoad = false;
                var cache = _database.GetMorpherCache();

                if (cache != null)
                {
                    foreach (var keyValuePair in cache)
                    {
                        _cache.Set(keyValuePair.Key, keyValuePair.Value, new DateTimeOffset(DateTime.Today.AddDays(1)));
                    }
                }
            }

            var result = _apiThrottler.LoadIntoCache(context.Request);
            if (result != ApiThrottlingResult.Success)
            {
                context.Response.Headers.Add(
                    "Error-Code",
                    new[] { new ServiceErrorMessage(result.GenerateMorpherException()).Code.ToString() });
            }

            return Next.Invoke(context);
        }
    }
}