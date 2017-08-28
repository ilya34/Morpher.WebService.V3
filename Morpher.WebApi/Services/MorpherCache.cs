namespace Morpher.WebService.V3.Services
{
    using System.Collections.Specialized;
    using System.Runtime.Caching;
    using Morpher.WebService.V3.Services.Interfaces;

    public class MorpherCache : MemoryCache, IMorpherCache
    {
        public MorpherCache(string name, NameValueCollection config = null)
            : base(name, config)
        {
        }
    }
}