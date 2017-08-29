namespace Morpher.WebService.V3.General.Data.Services
{
    using System.Collections.Specialized;
    using System.Runtime.Caching;

    public class MorpherCache : MemoryCache, IMorpherCache
    {
        public MorpherCache(string name, NameValueCollection config = null)
            : base(name, config)
        {
        }
    }
}