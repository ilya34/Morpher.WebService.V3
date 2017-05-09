namespace Morpher.WebApi.Services
{
    using System;
    using System.Net.Http;

    using Morpher.WebApi.ApiThrottler;
    using Morpher.WebApi.Extensions;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Exceptions;
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
            MorpherCacheObject morpherCacheObject = this.GetQueryLimit(ip);

            // Если GetQueryLimit вернул null, значит IP адрес помечен в бд как Blocked
            if (morpherCacheObject == null)
            {
                return ApiThrottlingResult.IpBlocked;
            }

            if (this.morpherCache.Decrement(morpherCacheObject))
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
            MorpherCacheObject morpherCacheObject = this.GetQueryLimit(guid);
            paidUser = false;

            // Если morpherCacheObject null, то токен не был найден в кэше, или бд.
            if (morpherCacheObject == null)
            {
                return ApiThrottlingResult.TokenNotFound;
            }

            paidUser = morpherCacheObject.PaidUser;

            if (morpherCacheObject.Unlimited)
            {
                return ApiThrottlingResult.Success;
            }

            if (this.morpherCache.Decrement(morpherCacheObject))
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
            paidUser = false;
            Guid? guid = null;
            try
            {
                guid = httpRequest.GetToken();
            }
            catch (InvalidTokenFormat)
            {
                return ApiThrottlingResult.InvalidToken;
            }

            // Если токен не указан, выполняем тарификацию по IP
            if (!guid.HasValue)
            {
                return this.Throttle(httpRequest.GetClientIp());
            }

            // Выполяем тарификацию по токену
            return this.Throttle(guid.Value, out paidUser);
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
        public MorpherCacheObject GetQueryLimit(string ip)
        {
            object cache = this.morpherCache.Get(ip);

            if (cache != null)
            {
                return (MorpherCacheObject)cache;
            }

            if (this.morpherDatabase.IsIpBlocked(ip))
            {
                return null;
            }

            int limit = this.morpherDatabase.GetDefaultDailyQueryLimit();
            int query = this.morpherDatabase.GetQueryCountByIp(ip);
            limit -= query;

            // Записываем  объект в кэш.
            MorpherCacheObject morpherCacheObject = new MorpherCacheObject() { QueriesLeft = limit, PaidUser = false, Unlimited = false };

            this.morpherCache.Set(ip, morpherCacheObject, this.absoluteExpiration);

            return morpherCacheObject;
        }

        /// <summary>
        /// Получает объект кэша по token
        /// </summary>
        /// Если объект не доступен в кэше. 
        /// Объект будет загружен из базы, и помещен в кэш.
        /// <param name="guid">Токен клиента</param>
        /// <returns>Объект кэша</returns>
        public MorpherCacheObject GetQueryLimit(Guid guid)
        {
            object obj = this.morpherCache.Get(guid.ToString().ToLowerInvariant());

            if (obj != null)
            {
                return (MorpherCacheObject)obj;
            }

            // Если объекта нет в кэше, нужно проверить его в бд.
            MorpherCacheObject morpherCacheObject = this.morpherDatabase.GetUserLimits(guid);
            if (morpherCacheObject == null)
            {
                return null;
            }

            if (morpherCacheObject.Unlimited)
            {
                morpherCacheObject.QueriesLeft = 1000;
            }
            else
            {
                int queries = this.morpherDatabase.GetQueryCountByToken(guid);
                morpherCacheObject.QueriesLeft -= queries;
            }

            this.morpherCache.Set(guid.ToString().ToLowerInvariant(), morpherCacheObject, this.absoluteExpiration);
            return morpherCacheObject;
        }
    }
}