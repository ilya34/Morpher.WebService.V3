namespace Morpher.WebApi.Services.Interfaces
{
    using System.Collections.Concurrent;
    using System.Net.Http;

    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Exceptions;

    public interface IMorpherLog
    {
        void Log(HttpRequestMessage message, MorpherException exception = null);

        void Sync();
    }
}