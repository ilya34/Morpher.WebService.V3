using System;
using System.Net.Http.Headers;
using System.Web;
using System.Xml.Serialization;

namespace Morpher.WebService.V3.General.Data
{
    using System.IO;
    using System.Net.Http;
    using System.Web.Http.Filters;
    using Autofac.Integration.WebApi;
    using Exceptions;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class MorpherExceptionFilterAttribute : ExceptionFilterAttribute, IAutofacExceptionFilter
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception as MorpherException ?? new ServerException(context.Exception);
            var responseFormat = HttpContext.Current.Request.GetResponseFormat();

            var responseObject = new ServiceErrorMessage(exception);
            context.Response = new HttpResponseMessage();
            if (!context.Response.Headers.Contains("Error-Code"))
            {
                context.Response.Headers.Add("Error-Code", new[] { responseObject.Code.ToString() });
                context.Response.StatusCode = exception.ResponseCode;
            }

            switch (responseFormat)
            {
                case ResponseFormat.Json:
                    context.Response.Content =
                        new StringContent(
                            JsonConvert.SerializeObject(responseObject, Formatting.Indented));
                    context.Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    break;
                case ResponseFormat.Xml:
                    XmlSerializer contractSerializer = new XmlSerializer(typeof(ServiceErrorMessage));
                    using (StringWriter stringWriter = new StringWriter())
                    {
                        contractSerializer.Serialize(stringWriter, responseObject);
                        context.Response.Content = new StringContent(stringWriter.GetStringBuilder().ToString());
                    }
                    context.Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}