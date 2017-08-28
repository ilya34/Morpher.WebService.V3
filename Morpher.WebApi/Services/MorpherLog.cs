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

        public void Sync()
        {
            this._database.Upload(this._logQueue);
        }
    }
}