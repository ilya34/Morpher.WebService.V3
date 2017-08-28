namespace Morpher.WebService.V3.Middlewares
{
    using System.Threading.Tasks;
    using Microsoft.Owin;

    public class GuidInsertMiddleware : OwinMiddleware
    {
        public GuidInsertMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            if (context.Request.Headers.Get("Authorization") != null)
            {
                context.Request.Headers.Remove("Authorization");
            }

            context.Request.Headers.Add("Authorization", new[] { "Basic ZTRjN2Q0Y2EtODAzNy00NGZhLWEzMTQtMmIxNzE3YzYyNmQ4Cg==" });

            return Next.Invoke(context);
        }
    }
}