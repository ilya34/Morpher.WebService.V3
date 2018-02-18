namespace Morpher.WebService.V3.General.Data
{
    using System.Collections.Concurrent;

    public interface IDatabaseLog
    {
        void Upload(ConcurrentQueue<LogEntry> logs);
    }
}