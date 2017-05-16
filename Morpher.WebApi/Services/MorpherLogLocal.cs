namespace Morpher.WebService.V3.Services
{
    using System.Collections.Concurrent;
    using System.Net.Http;

    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Models.Exceptions;
    using Morpher.WebService.V3.Services.Interfaces;

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