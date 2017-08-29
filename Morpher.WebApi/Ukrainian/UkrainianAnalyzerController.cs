namespace Morpher.WebService.V3.Ukrainian
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Data;
    using General.Data;

    [RoutePrefix("ukrainian")]
    public class UkrainianAnalyzerController : ApiController
    {
        private readonly IUkrainianAnalyzer _analyzer;
        private readonly IResultTrimmer _resultTrimmer;

        public UkrainianAnalyzerController(
            IUkrainianAnalyzer analyzer,
            IResultTrimmer resultTrimmer)
        {
            _analyzer = analyzer;
            _resultTrimmer = resultTrimmer;
        }

        [Route("declension", Name = "UkrainianDeclension")]
        [ThrottleThis]
        [LogThis]
        [HttpGet]
        public HttpResponseMessage Declension(string s, DeclensionFlags? flags = null, ResponseFormat? format = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new RequiredParameterIsNotSpecifiedException(nameof(s));
            }

            Data.DeclensionResult declensionResult =
                _analyzer.Declension(s, flags);

            _resultTrimmer.Trim(declensionResult, Request.GetToken());

            return Request.CreateResponse(HttpStatusCode.OK, declensionResult, format);

        }

        [Route("spell")]
        [ThrottleThis]
        [LogThis]
        [HttpGet]
        public HttpResponseMessage Spell(int n, string unit, ResponseFormat? format = null)
        {
            if (string.IsNullOrWhiteSpace(unit))
            {
                throw new RequiredParameterIsNotSpecifiedException(nameof(unit));
            }

            NumberSpelling numberSpelling =
                    _analyzer.Spell(n, unit);
            return Request.CreateResponse(HttpStatusCode.OK, numberSpelling, format);
        }
    }
}