namespace Morpher.WebService.V3.Services
{
    using System.Web.Hosting;

    using FluentScheduler;

    using Morpher.WebService.V3.Services.Interfaces;

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
            lock (this.lockObject)
            {
                if (this.shuttingDown)
                {
                    return;
                }

                _morpherLog.Sync();
            }
        }

        public void Stop(bool immediate)
        {
            lock (this.lockObject)
            {
                this.shuttingDown = true;
            }

            HostingEnvironment.UnregisterObject(this);
        }
    }
}