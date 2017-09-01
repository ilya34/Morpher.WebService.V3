namespace Morpher.WebService.V3.General.Data.Services
{
    using System.Collections.Specialized;
    using System.Runtime.Caching;
    using Interfaces;

    public class MorpherCache : MemoryCache, ICorrectionCache, IMorpherCache
    {
        public MorpherCache(string name, NameValueCollection config = null)
            : base(name, config)
        {
        }
    }
}