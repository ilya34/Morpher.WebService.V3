namespace Morpher.WebService.V3.Services.Interfaces
{
    using System.Collections.Concurrent;

    using Morpher.WebService.V3.Models;

    public interface IDatabaseLog
    {
        void Upload(ConcurrentQueue<LogEntity> logs);
    }
}