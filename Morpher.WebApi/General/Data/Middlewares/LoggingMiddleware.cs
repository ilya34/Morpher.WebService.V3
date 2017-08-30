namespace Morpher.WebService.V3.General.Data
{
    using System.Threading.Tasks;
    using Autofac.Features.AttributeFilters;
    using Microsoft.Owin;

    public class LoggingMiddleware : OwinMiddleware
    {
        private readonly IMorpherLog _morpherLog;
        private readonly IAttributeUrls _attributeUrls;

        public LoggingMiddleware(
            OwinMiddleware next,
            IMorpherLog morpherLog,
            IAttributeUrls attributeUrls) : base(next)
        {
            _morpherLog = morpherLog;
            _attributeUrls = attributeUrls;
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (context.Response.Headers.Get("Error-Code") == null)
            {
                await Next.Invoke(context);
            }

            if (_attributeUrls.Urls.Contains(context.Request.Path.ToString().ToLowerInvariant()))
            {
                _morpherLog.Log(context);
            }
        }
    }
}