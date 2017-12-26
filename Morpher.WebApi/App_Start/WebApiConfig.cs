namespace Morpher.WebService.V3
{
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;

    using Elmah.Contrib.WebApi;
    using General;

    public static class WebApiConfig
    {
        public static PlainTextFormatter PlainTextFormatter;

        public static XmlMediaTypeFormatter XmlMediaTypeFormatter;

        public static JsonMediaTypeFormatter JsonMediaTypeFormatter;

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            PlainTextFormatter = new PlainTextFormatter();
            XmlMediaTypeFormatter = config.Formatters.XmlFormatter;
            JsonMediaTypeFormatter = config.Formatters.JsonFormatter;

            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
            config.Formatters.Add(PlainTextFormatter);

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
