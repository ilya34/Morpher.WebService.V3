namespace Morpher.WebService.V3.General.Data.Services
{
    using System;
    using System.Collections.Concurrent;
    using Microsoft.Owin;

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
            if (errorCode != new InvalidTokenFormatException().Code)
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
            _database.Upload(_logQueue);
        }
    }
}