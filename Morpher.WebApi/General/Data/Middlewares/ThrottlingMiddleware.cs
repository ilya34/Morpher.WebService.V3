namespace Morpher.WebService.V3.General.Data
{
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Newtonsoft.Json;

    public class ThrottlingMiddleware : OwinMiddleware
    {
        private readonly IApiThrottler _apiThrottler;
        private readonly IAttributeUrls _attributeUrls;

        public ThrottlingMiddleware(
            OwinMiddleware next,
            IApiThrottler apiThrottler,
            IAttributeUrls attributeUrls) : base(next)
        {
            _apiThrottler = apiThrottler;
            _attributeUrls = attributeUrls;
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (_attributeUrls.Urls.Contains(context.Request.Path.ToString().ToLowerInvariant()))
            {

                ApiThrottlingResult result = _apiThrottler.Throttle(context.Request);

                if (result != ApiThrottlingResult.Success)
                {
                    var response = new ServiceErrorMessage(result.GenerateMorpherException());
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

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

                    context.Response.Headers.Add("Error-Code", new[] { response.Code.ToString() });
                    await Next.Invoke(context);
                    return;
                }
            }

            await Next.Invoke(context);
        }
    }
}