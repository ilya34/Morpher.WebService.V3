namespace Morpher.WebService.V3.Services.Interfaces
{
    using System.Net.Http;

    using Morpher.WebService.V3.Models.Exceptions;

    public interface IMorpherLog
    {
        void Log(HttpRequestMessage message, MorpherException exception = null);

        void Sync();
    }
}