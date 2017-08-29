namespace Morpher.WebService.V3
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentScheduler;
    using General.Data;
    using General.Data.Services;

    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly bool isLocal = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]);

        protected void Application_Start()
        {
            AutofacInit.Init();

            if (!isLocal)
            {
                NameValueCollection conf = (NameValueCollection)ConfigurationManager.GetSection("WebServiceSettings");
                int everyMinutes = Convert.ToInt32(conf["SyncCacheEveryMinutes"]);
                Registry registry = new Registry();
                JobManager.JobFactory = new JobFactory();
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
            if (!isLocal)
            {
                IMorpherLog log =
                    (IMorpherLog) AutofacInit.AutofacWebApiDependencyResolver.GetService(typeof(IMorpherLog));
                log.Sync();
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
        }
    }
}