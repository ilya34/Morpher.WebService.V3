namespace Morpher.WebService.V3.General.Data.Services
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Runtime.Caching;
    using Interfaces;

    public class MorpherCache : MemoryCache, ICorrectionCache, IMorpherCache
    {
        public MorpherCache(string name, NameValueCollection config = null)
            : base(name, config)
        {
        }


        public List<KeyValuePair<string, object>> GetAll()
        {
            return  this.ToList();
        }
    }
}