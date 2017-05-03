namespace Morpher.WebApi.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net.Http;

    using Morpher.WebApi.Extensions;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Exceptions;
    using Morpher.WebApi.Services.Interfaces;

    public class MorpherLog : IMorpherLog
    {
        private readonly IDatabaseLog database;

        private readonly ConcurrentQueue<LogEntity> logQueue = new ConcurrentQueue<LogEntity>();

        public MorpherLog(IDatabaseLog database)
        {
            this.database = database;
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
            Guid? token = message.GetToken();

            this.logQueue.Enqueue(
                new LogEntity(remoteAddress, queryString, querySource, DateTime.UtcNow, token, userAgent, errorCode));
        }

        public void Sync()
        {
            this.database.Upload(this.logQueue);
        }
    }
}