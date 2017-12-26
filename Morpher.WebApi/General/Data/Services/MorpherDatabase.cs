namespace Morpher.WebService.V3.General
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Dapper;
    using Newtonsoft.Json;

    public class MorpherDatabase : IMorpherDatabase
    {
        private readonly string _connectionString;

        public MorpherDatabase(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public int GetDefaultDailyQueryLimit()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingleOrDefault<int>("SELECT TOP 1 DailyQueryLimit FROM WebServiceSettings");
            }
        }

        public List<KeyValuePair<string, MorpherCacheObject>> GetMorpherCache()
        {
            using (CompressedCacheDataContext context = new CompressedCacheDataContext())
            {
                var result = context.CompressedCaches.FirstOrDefault(cache => cache.Date == DateTime.Now.Date);
                if (result != null)
                {
                    return JsonConvert.DeserializeObject<List<KeyValuePair<string, MorpherCacheObject>>>(Gzip.UnZip(result.GZipCache));
                }

                return null;
            }
        }

        public void UploadMorpherCache(List<KeyValuePair<string, MorpherCacheObject>> cache)
        {

            List<KeyValuePair<string, MorpherCacheObject>> pairs =
                cache.Select(   
                    pair => new KeyValuePair<string, MorpherCacheObject>(pair.Key, (MorpherCacheObject)pair.Value)).ToList();

            string serializedCache = Gzip.Zip(JsonConvert.SerializeObject(pairs));

            using (CompressedCacheDataContext context = new CompressedCacheDataContext())
            {
                var result =
                    context.CompressedCaches.FirstOrDefault(
                        compressedCache => compressedCache.Date == DateTime.Now.Date);

                if (result != null)
                {
                    result.GZipCache = serializedCache;
                }
                else
                {
                    context.CompressedCaches.InsertOnSubmit(new CompressedCache()
                    {
                        Date = DateTime.Now.Date,
                        GZipCache = serializedCache
                    });
                }

                context.SubmitChanges();
            }
        }

        public MorpherCacheObject GetUserLimits(Guid guid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<MorpherCacheObject>(
                    "sp_GetLimit",
                    new { Token = guid },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public bool IsIpBlocked(string ip)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<bool>(
                    "SELECT Blocked FROM RemoteAddresses WHERE REMOTE_ADDR = @ip",
                    new { ip });
            }
        }
    }
}