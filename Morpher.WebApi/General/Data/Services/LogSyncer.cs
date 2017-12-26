namespace Morpher.WebService.V3.General
{
    using System.Web.Hosting;
    using FluentScheduler;

    public class LogSyncer : IJob, IRegisteredObject
    {
        private readonly IMorpherLog _morpherLog;

        private readonly object lockObject = new object();

        private bool shuttingDown;

        public LogSyncer(IMorpherLog morpherLog)
        {
            _morpherLog = morpherLog;
            // Register this job with the hosting environment.
            // Allows for a more graceful stop of the job, in the case of IIS shutting down.
            HostingEnvironment.RegisterObject(this);
        }

        public void Execute()
        {
            lock (lockObject)
            {
                if (shuttingDown)
                {
                    return;
                }

                _morpherLog.Sync();
            }
        }

        public void Stop(bool immediate)
        {
            lock (lockObject)
            {
                shuttingDown = true;
            }

            HostingEnvironment.UnregisterObject(this);
        }
    }
}