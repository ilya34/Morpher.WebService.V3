namespace Morpher.WebApi.Services
{
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

        public bool Decrement(string key)
        {
            CacheObject cacheObject = (CacheObject)this.Get(key);

            return Interlocked.Decrement(ref cacheObject.DailyLimit) > 0;
        }
    }
}