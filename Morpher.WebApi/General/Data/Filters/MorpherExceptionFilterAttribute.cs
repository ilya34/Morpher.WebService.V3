namespace Morpher.WebService.V3.General.Data
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using System.Web.Http.Filters;
    using System.Xml;
    using Autofac.Integration.WebApi;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class MorpherExceptionFilterAttribute : ExceptionFilterAttribute, IAutofacExceptionFilter
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception as MorpherException;
            if (exception == null)
            {
                return;
            }

            var format = context.Request.GetQueryString("format");

            if (format == null)
            {
                format = context.Request.GetHeader("ContentType");

                if (format == null)
                {
                    format = "xml";
                }
                else if (format.Contains("application/json"))
                {
                    format = "json";
                }
                else if (format.Contains("application/xml"))
                {
                    format = "xml";
                }
            }

            var response = new ServiceErrorMessage(exception);
            context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            context.Response.Headers.Add("Error-Code", new[] { response.Code.ToString() });

            switch (format)
            {
                case "json":

                    context.Response.Content =
                        new StringContent(
                            JsonConvert.SerializeObject(response, Formatting.Indented));
                    break;
                case "xml":
                default:
                    DataContractSerializer contractSerializer = new DataContractSerializer(typeof(ServiceErrorMessage));
                    // Если писать поток в StreamContent ничего не выводит
                    // Если из ByteArray то кривая кодировка.
                    using (StringWriter stringWriter = new StringWriter())
                    {
                        using (XmlTextWriter textWriter =
                            new XmlTextWriter(stringWriter))
                        {
                            textWriter.Formatting = System.Xml.Formatting.Indented;
                            contractSerializer.WriteObject(textWriter, response);
                            context.Response.Content = new StringContent(stringWriter.GetStringBuilder().ToString());
                        }
                    }
                    break;
            }
        }
    }
}