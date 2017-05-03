namespace Morpher.WebApi.Services.Interfaces
{
    using System.Collections.Concurrent;
    using System.Data;

    using Morpher.WebApi.Models;

    public interface IDatabaseLog
    {
        void Upload(ConcurrentQueue<LogEntity> logs);
    }
}