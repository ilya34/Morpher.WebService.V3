namespace Morpher.WebApi
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Newtonsoft.Json;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

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
    }
}
