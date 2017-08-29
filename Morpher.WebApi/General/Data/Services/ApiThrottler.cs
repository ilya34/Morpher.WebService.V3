namespace Morpher.WebService.V3.General.Data.Services
{
    using System;
    using System.Threading;
    using Microsoft.Owin;

    public class ApiThrottler : IApiThrottler
    {
        private readonly IMorpherDatabase _morpherDatabase;

        private readonly IMorpherCache _morpherCache;

        private readonly DateTimeOffset _absoluteExpiration = new DateTimeOffset(DateTime.Today.AddDays(1));

        public ApiThrottler(IMorpherDatabase morpherDatabase, IMorpherCache morpherCache)
        {
            _morpherDatabase = morpherDatabase;
            _morpherCache = morpherCache;
        }

        /// <summary>
        /// Выполняет тарификацию по IP адресу
        /// </summary>
        /// <param name="ip">ip адрес клиента</param>
        /// <returns>Результат тарификации</returns>
        public ApiThrottlingResult Throttle(string ip)
        {
            MorpherCacheObject morpherCacheObject = GetQueryLimit(ip);

            // Если GetQueryLimit вернул null, значит IP адрес помечен в бд как Blocked
            if (morpherCacheObject == null)
            {
                return ApiThrottlingResult.IpBlocked;
            }

            return Decrement(morpherCacheObject);
        }

        static ApiThrottlingResult Decrement(MorpherCacheObject morpherCacheObject)
        {
            return Interlocked.Decrement(ref morpherCacheObject.QueriesLeft) >= 0 
                ? ApiThrottlingResult.Success 
                : ApiThrottlingResult.Overlimit;
        }

        /// <summary>
        /// Выполняет тарификацию по токену
        /// </summary>
        /// <param name="guid">Токен клиента</param>
        /// <returns>Результат тарификации</returns>
        public ApiThrottlingResult Throttle(Guid guid)
        {
            MorpherCacheObject morpherCacheObject = GetQueryLimit(guid);

            // Если morpherCacheObject null, то токен не был найден в кэше, или бд.
            if (morpherCacheObject == null)
            {
                return ApiThrottlingResult.TokenNotFound;
            }

            if (morpherCacheObject.Unlimited)
            {
                return ApiThrottlingResult.Success;
            }

            return Decrement(morpherCacheObject);
        }

        public ApiThrottlingResult Throttle(IOwinRequest request)
        {        
            try
            {
                Guid? token = request.GetToken();
                if (token != null)
                {
                    return Throttle(token.Value);
                }
                else
                {
                    string ip = request.RemoteIpAddress;
                    return Throttle(ip);
                }
            }
            catch (InvalidTokenFormatException)
            {
                return ApiThrottlingResult.InvalidToken;
            }
        }

        /// <summary>
        /// Удаляет клиента из кэша
        /// </summary>
        /// <param name="key">Токен клиента</param>
        /// <returns>Если запись найдена в кэше, удаленная запись кэша; в противном случае — значение null.</returns>
        public object RemoveFromCache(string key)
        {
            return _morpherCache.Remove(key);
        }

        /// <summary>
        /// Получает объет кэша по ip.
        /// </summary>
        /// <param name="ip">ip клиента</param>
        /// <returns>Запись в кэше; Если ip заблокирован - значение null.</returns>
        public MorpherCacheObject GetQueryLimit(string ip)
        {
            object cache = _morpherCache.Get(ip);

            if (cache != null)
            {
                return (MorpherCacheObject)cache;
            }

            if (_morpherDatabase.IsIpBlocked(ip))
            {
                return null;
            }

            int limit = _morpherDatabase.GetDefaultDailyQueryLimit();
            int query = _morpherDatabase.GetQueryCountByIp(ip);
            limit -= query;

            // Записываем  объект в кэш.
            MorpherCacheObject morpherCacheObject = new MorpherCacheObject() { QueriesLeft = limit, PaidUser = false, Unlimited = false };

            _morpherCache.Set(ip, morpherCacheObject, _absoluteExpiration);

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
            object obj = _morpherCache.Get(guid.ToString().ToLowerInvariant());

            if (obj != null)
            {
                return (MorpherCacheObject)obj;
            }

            // Если объекта нет в кэше, нужно проверить его в бд.
            MorpherCacheObject morpherCacheObject = _morpherDatabase.GetUserLimits(guid);
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
                int queries = _morpherDatabase.GetQueryCountByToken(guid);
                morpherCacheObject.QueriesLeft -= queries;
            }

            _morpherCache.Set(guid.ToString().ToLowerInvariant(), morpherCacheObject, _absoluteExpiration);
            return morpherCacheObject;
        }
    }
}