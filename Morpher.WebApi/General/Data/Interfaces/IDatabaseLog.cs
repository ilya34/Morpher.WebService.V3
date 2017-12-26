namespace Morpher.WebService.V3.General
{
    using System.Collections.Concurrent;

    public interface IDatabaseLog
    {
        void Upload(ConcurrentQueue<LogEntity> logs);
    }
}