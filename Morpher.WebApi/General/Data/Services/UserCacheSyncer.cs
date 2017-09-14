namespace Morpher.WebService.V3.General.Data.Services
{
    using System.Web.Hosting;
    using FluentScheduler;

    public class UserCacheSyncer : IJob, IRegisteredObject
    {
        private readonly IMorpherDatabase _morpherDatabase;
        private readonly IMorpherCache _morpherCache;
        private readonly object _lockObject = new object();

        private bool _shuttingDown;

        public UserCacheSyncer(IMorpherDatabase morpherDatabase, IMorpherCache morpherCache)
        {
            _morpherDatabase = morpherDatabase;
            _morpherCache = morpherCache;
            // Register this job with the hosting environment.
            // Allows for a more graceful stop of the job, in the case of IIS shutting down.
            HostingEnvironment.RegisterObject(this);
        }

        public void Execute()
        {
            lock (_lockObject)
            {
                if (_shuttingDown)
                {
                    return;
                }

                _morpherDatabase.UploadMorpherCache(_morpherCache.GetAll());
            }
        }

        public void Stop(bool immediate)
        {
            lock (_lockObject)
            {
                _shuttingDown = true;
            }

            HostingEnvironment.UnregisterObject(this);
        }
    }
}