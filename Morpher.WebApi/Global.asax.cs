namespace Morpher.WebApi
{
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Dapper;

    using FluentScheduler;

    using Morpher.WebApi.Models;
    using Morpher.WebApi.Services;

    using Newtonsoft.Json;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            var registry = new Registry();
            var sampleJob = new LogSyncer();
            registry.Schedule(() => Debug.WriteLine("Sort of executed")).ToRunEvery(30).Seconds();

            registry.Schedule<LogSyncer>().ToRunEvery(30).Seconds();

            JobManager.Initialize(registry);
            
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings = new JsonSerializerSettings
                                               {
                                                   Formatting = Formatting.Indented,
                                                   TypeNameHandling = TypeNameHandling.Objects,
                                                   //ContractResolver = new CamelCasePropertyNamesContractResolver()
                                               };


            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_End()
        {
            // TODO: Sync cache
        }
    }
}
