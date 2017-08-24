namespace Morpher.WebService.V3.Services.Interfaces
{
    using System.Net.Http;
    using Microsoft.Owin;
    using Models.Exceptions;

    public interface IMorpherLog
    {
        void Log(HttpRequestMessage message, MorpherException exception = null);

        void Log(IOwinContext context);

        void Sync();
    }
}