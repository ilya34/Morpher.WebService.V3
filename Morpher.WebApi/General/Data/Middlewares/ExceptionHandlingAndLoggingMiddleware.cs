using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Owin;
using Morpher.WebService.V3.General.Data.Exceptions;
using Newtonsoft.Json;

namespace Morpher.WebService.V3.General.Data
{
    public class ExceptionHandlingAndLoggingMiddleware : OwinMiddleware
    {
        private readonly IMorpherLog _morpherLog;
        private readonly IAttributeUrls _attributeUrls;

        public ExceptionHandlingAndLoggingMiddleware(
            OwinMiddleware next,
            IMorpherLog morpherLog,
            IAttributeUrls attributeUrls) : base(next)
        {
            _morpherLog = morpherLog;
            _attributeUrls = attributeUrls;
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
                context.Response.StatusCode = (int)exception.ResponseCode;
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

                context.Response.Headers.Set("Error-Code", exception.Code.ToString());
            }
            finally
            {
                string method = $"{context.Request.Method.ToLowerInvariant()}:{context.Request.Path.ToString().ToLowerInvariant()}";
                if (_attributeUrls.Urls.ContainsKey(method))
                {
                    _morpherLog.Log(context);
                }
            }
        }
    }
}