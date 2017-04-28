namespace Morpher.WebApi.Services.Interfaces
{
    using System.Net.Http;

    using Morpher.WebApi.Models.Exceptions;

    public class MorpherLogLocal : IMorpherLog
    {
        public void Log(HttpRequestMessage message, MorpherException exception = null)
        {
            return;
        }
    }
}