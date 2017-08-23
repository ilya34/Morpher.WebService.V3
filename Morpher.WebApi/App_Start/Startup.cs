using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Morpher.WebService.V3.App_Start.Startup))]

namespace Morpher.WebService.V3.App_Start
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAutofacMiddleware(AutofacInit.Container);
        }
    }
}
