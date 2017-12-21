namespace Morpher.WebService.V3.General.Data
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using System.Web.Http.Filters;
    using System.Xml;
    using Autofac.Integration.WebApi;
    using Exceptions;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class MorpherExceptionFilterAttribute : ExceptionFilterAttribute, IAutofacExceptionFilter
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception as MorpherException ?? new ServerException(context.Exception);

            var format = context.Request.GetQueryString("format") ?? context.Request.GetHeader("Accept");

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

            var response = new ServiceErrorMessage(exception);
            context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            if (!context.Response.Headers.Contains("Error-Code"))
            {
                context.Response.Headers.Add("Error-Code", new[] {response.Code.ToString()});
                context.Response.StatusCode = exception.ResponseCode;
            }

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