namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Newtonsoft.Json;

    public class ThrottlingMiddleware : OwinMiddleware
    {
        private readonly IApiThrottler _apiThrottler;
        private readonly IAttributeUrls _attributeUrls;

        private ThrottleThisAttribute _throttleThisAttribute;

        public ThrottlingMiddleware(
            OwinMiddleware next,
            IApiThrottler apiThrottler,
            IAttributeUrls attributeUrls) : base(next)
        {
            _apiThrottler = apiThrottler;
            _attributeUrls = attributeUrls;
        }

        private ApiThrottlingResult PerSymbol(IOwinRequest request)
        {
            StreamReader reader = new StreamReader(request.Body, Encoding.UTF8);
            var value = reader.ReadToEnd();
            request.Body.Position = 0;
            if (value == null) throw new RequiredParameterIsNotSpecifiedException("Text not found");
            int requestCost = (int)Math.Ceiling((double)value.Length / _throttleThisAttribute.Cost);
            return _apiThrottler.Throttle(request, requestCost);
        }

        private ApiThrottlingResult PerWord(IOwinRequest request)
        {
            throw new NotImplementedException();
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (_attributeUrls.Urls.TryGetValue(context.Request.Path.ToString().ToLowerInvariant(), out _throttleThisAttribute))
            {
                ApiThrottlingResult result;
                switch (_throttleThisAttribute.Mode)
                {
                    case TarificationMode.PerRequest:
                        result = _apiThrottler.Throttle(context.Request, _throttleThisAttribute.Cost);
                        break;
                    case TarificationMode.PerSymbol:
                        result = PerSymbol(context.Request);
                        break;
                    case TarificationMode.PerWord:
                        result = PerWord(context.Request);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (result != ApiThrottlingResult.Success)
                {
                    var exception = result.GenerateMorpherException();
                    var response = new ServiceErrorMessage(exception);
                    context.Response.StatusCode = (int)exception.ResponseCode;

                    var format = context.Request.Query.Get("format");
                    if (format == null)
                    {
                        format = context.Request.Headers.Get("ContentType");

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

                    switch (format)
                    {
                        case "json":
                            context.Response.ContentType = "application/json; charset=utf-8";
                            context.Response.Write(JsonConvert.SerializeObject(response, Formatting.Indented));
                            break;
                        case "xml":
                        default:
                            context.Response.ContentType = "application/xml; charset=utf-8";
                            DataContractSerializer contractSerializer = new DataContractSerializer(typeof(ServiceErrorMessage));
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                contractSerializer.WriteObject(memoryStream, response);
                                context.Response.Write(memoryStream.ToArray());
                            }
                            break;
                    }

                    if (!context.Response.Headers.ContainsKey("Error-Code"))
                    {
                        context.Response.Headers.Add("Error-Code", new[] { response.Code.ToString() });
                    }

                    await Next.Invoke(context);
                    return;
                }
            }

            await Next.Invoke(context);
        }
    }
}