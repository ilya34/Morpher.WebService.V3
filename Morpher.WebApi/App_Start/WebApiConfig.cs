namespace Morpher.WebService.V3
{
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;

    using Elmah.Contrib.WebApi;
    using General.Data;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
            config.Formatters.Add(new PlainTextFormatter());

            config.Formatters.JsonFormatter.Indent = true;
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            config.Formatters.XmlFormatter.WriterSettings.OmitXmlDeclaration = false;

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
