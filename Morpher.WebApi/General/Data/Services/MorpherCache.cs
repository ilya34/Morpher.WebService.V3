using Newtonsoft.Json;

namespace Morpher.WebService.V3.General
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Runtime.Caching;

    public class MorpherCache : MemoryCache, ICorrectionCache, IMorpherCache
    {
        public MorpherCache(string name, NameValueCollection config = null)
            : base(name, config)
        {
        }

        public bool FirstLoad { get; set; } = true;

        public List<KeyValuePair<string, MorpherCacheObject>> GetAll()
        {
            return this.Select(pair => new KeyValuePair<string, MorpherCacheObject>(pair.Key, (MorpherCacheObject)pair.Value)).ToList();
        }

        public string GetAllAsJson() => JsonConvert.SerializeObject(GetAll());
    }
}