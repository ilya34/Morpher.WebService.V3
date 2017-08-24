namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Microsoft.Owin;
    using Models.Exceptions;
    using Morpher.WebService.V3.Extensions;
    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;

    public class MorpherLog : IMorpherLog
    {
        private readonly IDatabaseLog _database;

        private readonly IMorpherCache _morpherCache;

        private readonly ConcurrentQueue<LogEntity> _logQueue = new ConcurrentQueue<LogEntity>();

        public MorpherLog(IDatabaseLog database, IMorpherCache morpherCache)
        {
            _database = database;
            _morpherCache = morpherCache;
        }

        public void Log(IOwinContext context)
        {
            string remoteAddress = context.Request.RemoteIpAddress;
            string queryString = context.Request.QueryString.ToString();
            string urlPath = context.Request.Path.ToString();
            string userAgent = context.Request.Headers.Get("User-Agent");
            int errorCode;
            int.TryParse(context.Response.Headers.Get("Error-code"), out errorCode);

            Guid? token = null;
            MorpherCacheObject cacheObject = null;
            if (errorCode != new InvalidTokenFormat().Code)
            {
                token = context.Request.GetToken();

                if (token != null)
                {
                    cacheObject = (MorpherCacheObject) _morpherCache.Get(token.ToString().ToLowerInvariant());
                }
            }

            _logQueue.Enqueue(
                new LogEntity(remoteAddress, queryString, urlPath, DateTime.UtcNow, token, cacheObject?.UserId, userAgent, errorCode));
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
                    cacheObject = (MorpherCacheObject)this._morpherCache.Get(token.ToString().ToLowerInvariant());
                }
            }

            this._logQueue.Enqueue(
                new LogEntity(remoteAddress, queryString, querySource, DateTime.UtcNow, token, cacheObject?.UserId, userAgent, errorCode));
        }

        public void Sync()
        {
            this._database.Upload(this._logQueue);
        }
    }
}