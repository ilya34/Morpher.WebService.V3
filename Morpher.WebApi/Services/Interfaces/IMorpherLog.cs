namespace Morpher.WebService.V3.Services.Interfaces
{
    using System.Net.Http;
    using Models.Exceptions;

    public interface IMorpherLog
    {
        void Log(HttpRequestMessage message, MorpherException exception = null);

        void Sync();
    }
}