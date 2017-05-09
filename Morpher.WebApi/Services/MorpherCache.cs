namespace Morpher.WebApi.Services
{
    using System;
    using System.Collections.Specialized;
    using System.Runtime.Caching;
    using System.Threading;

    using Morpher.WebApi.Models;
    using Morpher.WebApi.Services.Interfaces;

    public class MorpherCache : MemoryCache, IMorpherCache
    {
        public MorpherCache(string name, NameValueCollection config = null)
            : base(name, config)
        {
        }

        public bool Decrement(MorpherCacheObject morpherCacheObject)
        {
            return Interlocked.Decrement(ref morpherCacheObject.QueriesLeft) >= 0;
        }
    }
}