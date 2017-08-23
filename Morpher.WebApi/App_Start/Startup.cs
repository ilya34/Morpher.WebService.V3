using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Morpher.WebService.V3.App_Start.Startup))]

namespace Morpher.WebService.V3.App_Start
{
    using Middlewares;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use<ThrottlingMiddleware>();
        }
    }
}
