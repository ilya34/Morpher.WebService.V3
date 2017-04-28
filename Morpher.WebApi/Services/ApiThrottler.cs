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

        /// <summary>
        /// Выполняет тарификацию по IP адресу
        /// </summary>
        /// <param name="ip">ip адрес клиента</param>
        /// <returns>Результат тарификации</returns>
        public ApiThrottlingResult Throttle(string ip)
        {
            CacheObject cacheObject = this.GetQueryLimit(ip);

            // Если GetQueryLimit вернул null, значит IP адрес помечен в бд как Blocked
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

        /// <summary>
        /// Выполняет тарификацию по токену
        /// </summary>
        /// <param name="guid">Токен клиента</param>
        /// <param name="paidUser">Существует для данного клиента активная подписка</param>
        /// <returns>Результат тарификации</returns>
        public ApiThrottlingResult Throttle(Guid guid, out bool paidUser)
        {
            CacheObject cacheObject = this.GetQueryLimit(guid);
            paidUser = false;

            // Если cacheObject null, то токен не был найден в кэше, или бд.
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

        /// <summary>
        /// Выполняет тарификацию
        /// </summary>
        /// <param name="httpRequest">Http запрос, из него будет получен токен, или ip адрес клиента</param>
        /// <param name="paidUser">Существует для данного клиента активная подписка</param>
        /// <returns>Результат тарификации</returns>
        public ApiThrottlingResult Throttle(HttpRequestMessage httpRequest, out bool paidUser)
        {
            string token = null;
            paidUser = false;

            token = httpRequest.GetQueryString("token") ?? httpRequest.GetBasicAuthorization();

            // Если токен не указан, выполняем тарификацию по IP
            if (string.IsNullOrEmpty(token))
            {
                return this.Throttle(httpRequest.GetClientIp());
            }

            if (!Guid.TryParse(token, out Guid guid))
            {
                return ApiThrottlingResult.InvalidToken;
            }

            // Выполяем тарификацию по токену
            return this.Throttle(guid, out paidUser);

        }

        /// <summary>
        /// Удаляет клиента из кэша
        /// </summary>
        /// <param name="key">Токен клиента</param>
        /// <returns>Успешность удаления клиента</returns>
        public bool DeleteFromCache(string key)
        {
            lock (this.lockObject)
            {
                if (this.memoryCache.Contains(key.ToLowerInvariant()))
                {
                    this.memoryCache.Remove(key.ToLowerInvariant());
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Получает объет кэша по ip.
        /// </summary>
        /// <param name="ip">ip клиента</param>
        /// <returns>Объект кэша</returns>
        public CacheObject GetQueryLimit(string ip)
        {
            CacheObject cacheObject = this.GetObjectFromCache(ip);

            if (cacheObject != null)
            {
                return cacheObject;
            }

            // Проверяем заблокирован ли ip адрес
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

                // Я думаю что клиенту не стоит видеть  отрицательное значение запросов.
                // Так как логи пишуться на все запросы, а после пересчета логов их может оказаться больше чем доступно для юзера.
                limit -= query;
                if (limit < 0)
                {
                    limit = 0;
                }

                // Записываем  объект в кэш.
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

        /// <summary>
        /// Получает объект кэша по token
        /// </summary>
        /// Если объект не доступен в кэше. 
        /// Объект будет загружен из базы, и помещен в кэш.
        /// <param name="guid">Токен клиента</param>
        /// <returns>Объект кэша</returns>
        public CacheObject GetQueryLimit(Guid guid)
        {
            CacheObject cacheObject = this.GetObjectFromCache(guid.ToString());

            if (cacheObject != null)
            {
                return cacheObject;
            }

            // Если объекта нет в кэше, нужно проверить его в бд.
            cacheObject = this.GetRecordFromDatabase(guid);
            if (cacheObject == null)
            {
                return null;
            }

            if (cacheObject.Unlimited)
            {
                cacheObject.DailyLimit = 1000;
            }
            else
            {
                int queries;

                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    queries = connection.QuerySingle<int>(
                        "sp_GetQueryCount",
                        new { Token = guid },
                        commandType: CommandType.StoredProcedure);
                }

                // Я думаю что клиенту не стоит видеть  отрицательное значение запросов.
                // Так как логи пишуться на все запросы, а после пересчета логов их может оказаться больше чем доступно для юзера.
                cacheObject.DailyLimit -= queries;
                if (cacheObject.DailyLimit < 0)
                {
                    cacheObject.DailyLimit = 0;
                }
            }

            this.SetObject(guid.ToString(), cacheObject);
            return cacheObject;
        }

        /// <summary>
        /// Записывает объект в кэш по ключу.
        /// </summary>
        /// <param name="key">Ключ кэша</param>
        /// <param name="cacheObject">Объект  кэша</param>
        public void SetObject(string key, CacheObject cacheObject)
        {
            lock (this.lockObject)
            {
                this.memoryCache.Set(key.ToLowerInvariant(), cacheObject, new DateTimeOffset(DateTime.Today.AddDays(1)));
            }
        }

        /// <summary>
        /// Возвращает объект из кэша
        /// </summary>
        /// <param name="key">Ключ объекта в кэше</param>
        /// <returns>Возвращает объект кеша</returns>
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

        /// <summary>
        /// Получает объект из бд
        /// </summary>
        /// <param name="guid">токен клиента</param>
        /// <returns>Объект кеша</returns>
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

        /// <summary>
        /// Уменьшает кол-во запросов для клиента
        /// </summary>
        /// <param name="key">ключ объекта в кэше</param>
        /// <returns>Возвращает <c>false</c> если кол-во попыток меньше или равно 0</returns>
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