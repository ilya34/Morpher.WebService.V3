namespace Morpher.WebApi
{
    using System;
    using System.Configuration;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    using FluentScheduler;

    using Morpher.WebApi.Services;
    using Morpher.WebApi.Services.Interfaces;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            bool isLocal = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]);
           
            if (!isLocal)
            {
                int everyMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["SyncCacheEveryMinutes"]);
                Registry registry = new Registry();
                registry.Schedule<LogSyncer>().ToRunEvery(everyMinutes).Minutes();
                JobManager.Initialize(registry);
            }

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_End()
        {
            IMorpherLog log =
                (IMorpherLog)DependencyResolver.Current.GetService(typeof(IMorpherLog));
            log.Sync();
        }
    }
}