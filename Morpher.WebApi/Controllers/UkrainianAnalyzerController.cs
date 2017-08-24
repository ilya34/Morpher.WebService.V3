namespace Morpher.WebService.V3.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Extensions;
    using Helpers;
    using Models;
    using Models.Exceptions;
    using Services.Interfaces;

    [RoutePrefix("ukrainian")]
    public class UkrainianAnalyzerController : ApiController
    {
        private readonly IUkrainianAnalyzer _analyzer;

        public UkrainianAnalyzerController(
            IUkrainianAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        [Route("declension", Name = "UkrainianDeclension")]
        [ThrottleThis]
        [LogThis]
        [HttpGet]
        public HttpResponseMessage Declension(string s, DeclensionFlags? flags = null, ResponseFormat? format = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new RequiredParameterIsNotSpecified(nameof(s));
            }

            UkrainianDeclensionResult declensionResult =
                _analyzer.Declension(s, flags);
            return this.Request.CreateResponse(HttpStatusCode.OK, declensionResult, format);

        }

        [Route("spell")]
        [ThrottleThis]
        [LogThis]
        [HttpGet]
        public HttpResponseMessage Spell(int n, string unit, ResponseFormat? format = null)
        {
            if (string.IsNullOrWhiteSpace(unit))
            {
                throw new RequiredParameterIsNotSpecified(nameof(unit));
            }

            UkrainianNumberSpelling numberSpelling =
                    _analyzer.Spell(n, unit);
            return this.Request.CreateResponse(HttpStatusCode.OK, numberSpelling, format);
        }
    }
}