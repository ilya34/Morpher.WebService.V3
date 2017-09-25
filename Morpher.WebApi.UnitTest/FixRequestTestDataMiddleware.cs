namespace Morpher.WebService.V3.UnitTests
{
    using System.Threading.Tasks;
    using Microsoft.Owin;

    public class FixRequestTestDataMiddleware : OwinMiddleware
    {
        public FixRequestTestDataMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            context.Request.RemoteIpAddress = "0.0.0.0";
            return Next.Invoke(context);
        }
    }
}
