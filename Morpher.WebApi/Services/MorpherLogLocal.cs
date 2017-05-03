namespace Morpher.WebApi.Services
{
    using System.Net.Http;

    using Morpher.WebApi.Models.Exceptions;
    using Morpher.WebApi.Services.Interfaces;

    public class MorpherLogLocal : IMorpherLog
    {
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