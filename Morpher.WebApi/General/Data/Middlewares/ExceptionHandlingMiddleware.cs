using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Owin;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Morpher.WebService.V3.General
{
    public class ExceptionHandlingMiddleware : OwinMiddleware
    {
        public ExceptionHandlingMiddleware(OwinMiddleware next) : base(next)
        {
        }

        ResponseFormat GetResponseFormat(IOwinContext context)
        {
            var requestedResponseFormat =
                context.Request.Query.Get("format") ?? context.Request.Headers.Get("Accept");
            ResponseFormat responseFormat = ResponseFormat.Xml;
            if (requestedResponseFormat != null
                && (requestedResponseFormat.ToLowerInvariant() == "json"
                    || requestedResponseFormat.Contains("application/json")))
                responseFormat = ResponseFormat.Json;
            return responseFormat;
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception exc)
            {
                var exception = exc as MorpherException ?? new ServerException(exc);
                ResponseFormat responseFormat = GetResponseFormat(context);
                var responseObject = new ServiceErrorMessage(exception);
                context.Response.StatusCode = (int) exception.ResponseCode;
                switch (responseFormat)
                {
                    case ResponseFormat.Xml:
                        context.Response.ContentType = "application/xml";
                        using (var sw = new StringWriter())
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(ServiceErrorMessage));
                            serializer.Serialize(sw, responseObject);
                            context.Response.Write(sw.ToString());
                        }

                        break;
                    case ResponseFormat.Json:
                        context.Response.ContentType = "application/json";
                        context.Response.Write(JsonConvert.SerializeObject(responseObject, Formatting.Indented));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}