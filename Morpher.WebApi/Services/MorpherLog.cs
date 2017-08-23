namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using Morpher.WebService.V3.Extensions;
    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Models.Exceptions;

    public class MorpherLog : IMorpherLog
    {
        private readonly IDatabaseLog database;

        private readonly IMorpherCache morpherCache;

        private readonly ConcurrentQueue<LogEntity> logQueue = new ConcurrentQueue<LogEntity>();

        public MorpherLog(IDatabaseLog database, IMorpherCache morpherCache)
        {
            this.database = database;
            this.morpherCache = morpherCache;
        }



        public void Log(HttpRequestMessage message, MorpherException exception = null)
        {
            // ip клиента
            string remoteAddress = message.GetClientIp();

            // запрос
            Dictionary<string, string> dictionary = message.GetQueryStrings();

            // источник запроса
            string querySource = new Uri(message.RequestUri.ToString()).AbsolutePath;

            int errorCode = exception?.Code ?? 0;
            string queryString = string.Empty;

            // Формируем строку ввида param=value;param=value
            if (dictionary != null)
            {
                queryString = string.Join(";", dictionary.Select(pair => $"{pair.Key}={pair.Value}"));
            }

            string userAgent = message.Headers.UserAgent?.ToString();

            Guid? token = null;
            MorpherCacheObject cacheObject = null;
            if (!(exception is InvalidTokenFormat))
            {
                token = message.GetToken();

                if (token != null)
                {
                    cacheObject = (MorpherCacheObject)this.morpherCache.Get(token.ToString().ToLowerInvariant());
                }
            }

            this.logQueue.Enqueue(
                new LogEntity(remoteAddress, queryString, querySource, DateTime.UtcNow, token, cacheObject?.UserId, userAgent, errorCode));
        }

        public void Sync()
        {
            this.database.Upload(this.logQueue);
        }
    }
}