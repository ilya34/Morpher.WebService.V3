namespace Morpher.WebService.V3.Services
{
    using System.Collections.Concurrent;
    using System.Net.Http;
    using Microsoft.Owin;
    using Models.Exceptions;
    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;

    public class MorpherLogLocal : IMorpherLog
    {
        public ConcurrentQueue<LogEntity> logQueue { get; }

        public void Log(HttpRequestMessage message, MorpherException exception = null)
        {
            return;
        }

        public void Log(IOwinContext context)
        {
            throw new System.NotImplementedException();
        }

        public void Sync()
        {
            return;
        }
    }
}