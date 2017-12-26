namespace Morpher.WebService.V3.General
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Newtonsoft.Json;

    public class ThrottlingMiddleware : OwinMiddleware
    {
        private readonly IApiThrottler _apiThrottler;
        private readonly IAttributeUrls _attributeUrls;

        private ThrottleThisAttribute _throttleThisAttribute;

        public ThrottlingMiddleware(
            OwinMiddleware next,
            IApiThrottler apiThrottler,
            IAttributeUrls attributeUrls) : base(next)
        {
            _apiThrottler = apiThrottler;
            _attributeUrls = attributeUrls;
        }

        private static string ReadBody(IOwinRequest request)
        {
            StreamReader reader = new StreamReader(request.Body, Encoding.UTF8);
            var value = reader.ReadToEnd();
            request.Body.Position = 0;
            return value;
        }

        private ApiThrottlingResult PerSymbol(IOwinRequest request)
        {
            var value = ReadBody(request);
            if (value == null) throw new RequiredParameterIsNotSpecifiedException("Text not found");
            int requestCost = (int)Math.Ceiling((double)value.Length / _throttleThisAttribute.Cost);
            return _apiThrottler.Throttle(request, requestCost);
        }

        private ApiThrottlingResult PerWord(IOwinRequest request)
        {
            var value = ReadBody(request);
            if (value == null) throw new RequiredParameterIsNotSpecifiedException("Text not found");
            int requestCost = (int)Math.Ceiling((double)value.Split('\n').Length / _throttleThisAttribute.Cost);
            return _apiThrottler.Throttle(request, requestCost);
        }

        public override async Task Invoke(IOwinContext context)
        {
            string method = $"{context.Request.Method.ToLowerInvariant()}:{context.Request.Path.ToString().ToLowerInvariant()}";
            if (_attributeUrls.Urls.TryGetValue(method, out _throttleThisAttribute))
            {
                ApiThrottlingResult result;
                switch (_throttleThisAttribute.Mode)
                {
                    case TarificationMode.PerRequest:
                        result = _apiThrottler.Throttle(context.Request, _throttleThisAttribute.Cost);
                        break;
                    case TarificationMode.PerSymbol:
                        result = PerSymbol(context.Request);
                        break;
                    case TarificationMode.PerWord:
                        result = PerWord(context.Request);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (result != ApiThrottlingResult.Success)
                {
                    throw result.GenerateMorpherException();
                }
            }

            await Next.Invoke(context);
        }
    }
}