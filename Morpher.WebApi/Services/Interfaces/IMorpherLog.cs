namespace Morpher.WebApi.Services.Interfaces
{
    using System.Net.Http;

    using Morpher.WebApi.Models.Exceptions;

    public interface IMorpherLog
    {
        void Log(HttpRequestMessage message, MorpherException exception = null);

        void Sync();
    }
}