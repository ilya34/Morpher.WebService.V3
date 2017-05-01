namespace Morpher.WebApi.Services
{
    using System;
    using System.Net.Http;

    using Morpher.WebApi.ApiThrottler;
    using Morpher.WebApi.Extensions;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Services.Interfaces;

    public class ApiThrottler : IApiThrottler
    {
        private readonly IMorpherDatabase morpherDatabase;

        private readonly IMorpherCache morpherCache;

        private readonly DateTimeOffset absoluteExpiration = new DateTimeOffset(DateTime.Today.AddDays(1));

        public ApiThrottler(IMorpherDatabase morpherDatabase, IMorpherCache morpherCache)
        {
            this.morpherDatabase = morpherDatabase;
            this.morpherCache = morpherCache;
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

            if (this.morpherCache.Decrement(ip))
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

            if (this.morpherCache.Decrement(guid.ToString().ToLowerInvariant()))
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

            Guid guid;
            if (!Guid.TryParse(token, out guid))
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
        /// <returns>Если запись найдена в кэше, удаленная запись кэша; в противном случае — значение null.</returns>
        public object RemoveFromCache(string key)
        {
            return this.morpherCache.Remove(key);
        }

        /// <summary>
        /// Получает объет кэша по ip.
        /// </summary>
        /// <param name="ip">ip клиента</param>
        /// <returns>Запись в кэше; Если ip заблокирован - значение null.</returns>
        public CacheObject GetQueryLimit(string ip)
        {
            object cache = this.morpherCache.Get(ip);

            if (cache != null)
            {
                return (CacheObject)cache;
            }

            if (this.morpherDatabase.IsIpBlocked(ip))
            {
                return null;
            }

            int limit = this.morpherDatabase.GetDefaultDailyQueryLimit();
            int query = this.morpherDatabase.GetQueryCountByIp(ip);

            // Я думаю что клиенту не стоит видеть  отрицательное значение запросов.
            // Так как логи пишуться на все запросы, а после пересчета логов их может оказаться больше чем доступно для юзера.
            limit -= query;
            if (limit < 0)
            {
                limit = 0;
            }

            // Записываем  объект в кэш.
            CacheObject cacheObject = new CacheObject() { DailyLimit = limit, PaidUser = false, Unlimited = false };

            this.morpherCache.Set(ip, cacheObject, this.absoluteExpiration);

            return cacheObject;
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
            object obj = this.morpherCache.Get(guid.ToString().ToLowerInvariant());

            if (obj != null)
            {
                return (CacheObject)obj;
            }

            // Если объекта нет в кэше, нужно проверить его в бд.
            CacheObject cacheObject = this.morpherDatabase.GetUserLimits(guid);
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
                int queries = this.morpherDatabase.GetQueryCountByToken(guid);

                // Я думаю что клиенту не стоит видеть  отрицательное значение запросов.
                // Так как логи пишуться на все запросы, а после пересчета логов их может оказаться больше чем доступно для юзера.
                cacheObject.DailyLimit -= queries;
                if (cacheObject.DailyLimit < 0)
                {
                    cacheObject.DailyLimit = 0;
                }
            }

            this.morpherCache.Set(guid.ToString().ToLowerInvariant(), cacheObject, this.absoluteExpiration);
            return cacheObject;
        }
    }
}