namespace Morpher.WebService.V3.General.Data.Middlewares
{
    using System.Threading.Tasks;
    using Microsoft.Owin;

    public class UserCacheLoaderMiddleware : OwinMiddleware
    {
        private readonly IApiThrottler _apiThrottler;

        public UserCacheLoaderMiddleware(
            OwinMiddleware next,
            IApiThrottler apiThrottler) : base(next)
        {
            _apiThrottler = apiThrottler;
        }

        public override Task Invoke(IOwinContext context)
        {
            var result = _apiThrottler.LoadIntoCache(context.Request);
            if (result != ApiThrottlingResult.Success)
            {
                context.Response.Headers.Add(
                    "Error-Code",
                    new[] { new ServiceErrorMessage(result.GenerateMorpherException()).Code.ToString() });
            }

            return Next.Invoke(context);
        }
    }
}