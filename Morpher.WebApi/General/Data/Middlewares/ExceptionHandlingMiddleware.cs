using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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


        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception exc)
            {
                var exception = exc as MorpherException ?? new ServerException(exc);
                var ctx = HttpContext.Current;
                ResponseFormat responseFormat = ctx.Request.GetResponseFormat();
                ctx.Response.Clear();
                ctx.Response.StatusCode = (int)exception.ResponseCode;
                var responseObject = new ServiceErrorMessage(exception);
                switch (responseFormat)
                {
                    case ResponseFormat.Xml:
                        ctx.Response.ContentType = "application/xml";
                        XmlSerializer serializer = new XmlSerializer(typeof(ServiceErrorMessage));
                        serializer.Serialize(ctx.Response.OutputStream, responseObject);
                        break;
                    case ResponseFormat.Json:
                        ctx.Response.ContentType = "application/json";
                        var byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseObject, Formatting.Indented));
                        await ctx.Response.OutputStream.WriteAsync(byteArray, 0, byteArray.Length);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}