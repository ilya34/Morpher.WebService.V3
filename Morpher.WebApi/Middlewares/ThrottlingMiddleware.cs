namespace Morpher.WebService.V3.Middlewares
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Helpers;
    using Microsoft.Owin;

    public class ThrottlingMiddleware : OwinMiddleware
    {


        public ThrottlingMiddleware(OwinMiddleware next) : base(next)
        {

        }

        public override async Task Invoke(IOwinContext context)
        {
            if (ThrottleUrls.Urls.Contains(context.Request.Path.ToString().ToLowerInvariant()))
            {
                Debug.WriteLine("HERE WE ARE");
            }

            await Next.Invoke(context);
        }
    }
}