namespace Morpher.WebApi.Services
{
    using System.Collections.Concurrent;
    using System.Net.Http;

    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Exceptions;
    using Morpher.WebApi.Services.Interfaces;

    public class MorpherLogLocal : IMorpherLog
    {
        public ConcurrentQueue<LogEntity> logQueue { get; }

        public void Log(HttpRequestMessage message, MorpherException exception = null)
        {
            return;
        }

        public void Sync()
        {
            return;
        }
    }
}