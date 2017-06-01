namespace Morpher.WebService.V3
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    using FluentScheduler;

    using Morpher.WebService.V3.Services;
    using Morpher.WebService.V3.Services.Interfaces;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            bool isLocal = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]);

            if (!isLocal)
            {
                NameValueCollection conf = (NameValueCollection)ConfigurationManager.GetSection("WebServiceSettings");
                int everyMinutes = Convert.ToInt32(conf["SyncCacheEveryMinutes"]);
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
            IMorpherLog log = (IMorpherLog)DependencyResolver.Current.GetService(typeof(IMorpherLog));
            log.Sync();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
        }
    }
}