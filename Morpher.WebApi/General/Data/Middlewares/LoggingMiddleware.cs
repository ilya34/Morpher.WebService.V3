namespace Morpher.WebService.V3.General
{
    using System.Threading.Tasks;
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

            string method = $"{context.Request.Method.ToLowerInvariant()}:{context.Request.Path.ToString().ToLowerInvariant()}";
            if (_attributeUrls.Urls.ContainsKey(method))
            {
                _morpherLog.Log(context);
            }
        }
    }
}