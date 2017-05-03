namespace Morpher.WebApi.Services
{
    using System.Web.Hosting;

    using FluentScheduler;

    using Morpher.WebApi.Services.Interfaces;

    public class LogSyncer : IJob, IRegisteredObject
    {
        private readonly object lockObject = new object();

        private bool shuttingDown;

        public LogSyncer()
        {
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

                IMorpherLog log =
                    (IMorpherLog)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IMorpherLog));
                log.Sync();
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