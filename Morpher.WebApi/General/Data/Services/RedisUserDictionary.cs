namespace Morpher.WebService.V3.General.Data
{
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using Newtonsoft.Json;
    using Russian.Data;
    using StackExchange.Redis;

    public class RedisUserDictionary : IUserDictionaryLookup, IExceptionDictionary
    {
        private readonly IMorpherCache _morpherCache;

        //private readonly ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(
        //    new ConfigurationOptions() { Password = "*********", EndPoints = { "88.99.171.113:6379" } });

        private readonly ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("localhost");

        private readonly IDatabase database;

        public RedisUserDictionary(IMorpherCache morpherCache)
        {
            _morpherCache = morpherCache;
            database = connection.GetDatabase();
        }

        public Entry Lookup(string nominativeSingular)
        {
            var token = HttpContext.Current.Request.GetToken();
            if (token == null)
            {
                return null;
            }

            var cache = (MorpherCacheObject)_morpherCache.Get(token.ToString().ToLowerInvariant());

            string normalizedLemma = LemmaNormalizer.Normalize(nominativeSingular);
            string toHash = $"{cache.UserId}:{normalizedLemma}:RU";

            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(toHash));
            //Entry entry = new Entry(new DeclensionForms() { Genitive = "редис тест" }, null);
            //database.StringSet(hash, JsonConvert.SerializeObject(entry));
            string value = database.StringGet(hash);
            if (value != null)
            {
                return JsonConvert.DeserializeObject<Entry>(value);
            }

            return null;
        }

        public void Add(CorrectionPostModel model)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(string nominativeForm)
        {
            throw new System.NotImplementedException();
        }

        public List<Entry> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}