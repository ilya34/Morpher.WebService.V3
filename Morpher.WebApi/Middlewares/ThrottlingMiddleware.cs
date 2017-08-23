namespace Morpher.WebService.V3.Middlewares
{
    using System.Net;
    using System.Threading.Tasks;
    using Extensions;
    using Helpers;
    using Microsoft.Owin;
    using Models;
    using Newtonsoft.Json;
    using Services.Interfaces;

    public class ThrottlingMiddleware : OwinMiddleware
    {
        private readonly IApiThrottler _apiThrottler;

        public ThrottlingMiddleware(OwinMiddleware next, IApiThrottler apiThrottler) : base(next)
        {
            _apiThrottler = apiThrottler;
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (ThrottleUrls.Urls.Contains(context.Request.Path.ToString().ToLowerInvariant()))
            {
                ApiThrottlingResult result = _apiThrottler.Throttle(context.Request);

                if (result != ApiThrottlingResult.Success)
                {
                    var response = new ServiceErrorMessage(result.GenerateMorpherException());
                    context.Response.StatusCode = (int)HttpStatusCode.PaymentRequired;
                    context.Response.ContentType = "application/json";
                    context.Response.Write(JsonConvert.SerializeObject(response));
                }
            }

            await Next.Invoke(context);
        }
    }
}