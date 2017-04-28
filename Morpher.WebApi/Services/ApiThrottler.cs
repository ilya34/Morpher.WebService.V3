namespace Morpher.WebApi.Services
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Net.Http;
    using System.Runtime.Caching;

    using Dapper;

    using Morpher.WebApi.ApiThrottler;
    using Morpher.WebApi.Extensions;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Services.Interfaces;

    public class ApiThrottler : IApiThrottler
    {
        private readonly MemoryCache memoryCache;

        private readonly string connectionString;

        private readonly object lockObject = new object();

        public ApiThrottler(string connectionString)
        {
            this.connectionString = connectionString;
            this.memoryCache = MemoryCache.Default;
        }

        public ApiThrottlingResult Throttle(string ip)
        {
            CacheObject cacheObject = this.GetQueryLimit(ip);

            if (cacheObject == null)
            {
                return ApiThrottlingResult.IpBlocked;
            }

            if (this.Decrement(ip))
            {
                return ApiThrottlingResult.Success;
            }

            return ApiThrottlingResult.Overlimit;

        }

        public ApiThrottlingResult Throttle(Guid guid, out bool paidUser)
        {
            CacheObject cacheObject = this.GetQueryLimit(guid);
            paidUser = false;
            if (cacheObject == null)
            {
                return ApiThrottlingResult.InvalidToken;
            }

            paidUser = cacheObject.PaidUser;
            if (cacheObject.Unlimited)
            {
                return ApiThrottlingResult.Success;
            }

            if (this.Decrement(guid.ToString()))
            {
                return ApiThrottlingResult.Success;
            }

            return ApiThrottlingResult.Overlimit;
        }

        public ApiThrottlingResult Throttle(HttpRequestMessage httpRequest, out bool paidUser)
        {
            string token = null;
            paidUser = false;

            token = httpRequest.GetQueryString("token") ?? httpRequest.GetBasicAuthorization();

            if (string.IsNullOrEmpty(token))
            {
                return this.Throttle(httpRequest.GetClientIp());
            }

            if (!Guid.TryParse(token, out Guid guid))
            {
                return ApiThrottlingResult.InvalidToken;
            }

            return this.Throttle(guid, out paidUser);

        }

        public bool UpdateCache(string key)
        {
            lock (this.lockObject)
            {
                if (this.memoryCache.Contains(key))
                {
                    this.memoryCache.Remove(key);
                    return true;
                }

                return false;
            }
        }

        public CacheObject GetQueryLimit(string ip)
        {
            CacheObject cacheObject = this.GetObjectFromCache(ip);

            if (cacheObject != null)
            {
                return cacheObject;
            }

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                if (connection.QuerySingleOrDefault<bool>(
                    "SELECT Blocked FROM RemoteAddresses WHERE REMOTE_ADDR = @ip",
                    new { ip }))
                {
                    return null;
                }

                int limit = connection.QuerySingleOrDefault<int>("SELECT TOP 1 DailyQueryLimit FROM WebServiceSettings");
                int query = connection.QuerySingle<int>(
                    "sp_GetQueryCountByIp",
                    new { Ip = ip },
                    commandType: CommandType.StoredProcedure);
                limit -= query;
                if (limit < 0)
                {
                    limit = 0;
                }

                cacheObject = new CacheObject()
                                  {
                                      DailyLimit = limit,
                                      PaidUser = false,
                                      Unlimited = false
                                  };

                this.SetObject(ip, cacheObject);

                return cacheObject;
            }
        }

        public CacheObject GetQueryLimit(Guid guid)
        {
            CacheObject cacheObject = this.GetObjectFromCache(guid.ToString());

            if (cacheObject != null)
            {
                return cacheObject;
            }

            cacheObject = this.GetRecordFromDatabase(guid);
            if (cacheObject == null)
            {
                return null;
            }

            if (cacheObject.Unlimited)
            {
                return cacheObject;
            }

            int queries;

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                queries = connection.QuerySingle<int>(
                    "sp_GetQueryCount",
                    new { Token = guid },
                    commandType: CommandType.StoredProcedure);
            }

            cacheObject.DailyLimit -= queries;
            if (cacheObject.DailyLimit < 0)
            {
                cacheObject.DailyLimit = 0;
            }

            this.SetObject(guid.ToString(), cacheObject);
            return cacheObject;
        }

        public void SetObject(string key, CacheObject cacheObject)
        {
            lock (this.lockObject)
            {
                this.memoryCache.Set(key, cacheObject, new DateTimeOffset(DateTime.Today.AddDays(1)));
            }
        }

        public CacheObject GetObjectFromCache(string key)
        {
            lock (this.lockObject)
            {
                if (this.memoryCache.Contains(key))
                {
                    return this.memoryCache.Get(key) as CacheObject;
                }
            }

            return null;
        }

        public CacheObject GetRecordFromDatabase(Guid guid)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                return connection.QueryFirstOrDefault<CacheObject>(
                    "sp_GetLimit",
                    new { Token = guid },
                    commandType: CommandType.StoredProcedure);
            }
        }

        private bool Decrement(string key)
        {
            lock (this.lockObject)
            {
                if (!this.memoryCache.Contains(key))
                {
                    return false;
                }

                CacheObject cacheObject = this.memoryCache.Get(key) as CacheObject;

                if (cacheObject?.DailyLimit > 0)
                {
                    cacheObject.DailyLimit--;
                    this.SetObject(key, cacheObject);
                    return true;
                }

                return false;
            }
        }
    }
}