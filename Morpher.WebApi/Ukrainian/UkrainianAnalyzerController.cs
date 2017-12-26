namespace Morpher.WebService.V3.Ukrainian
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Data;
    using General;

    [RoutePrefix("ukrainian")]
    public class UkrainianAnalyzerController : ApiController
    {
        private readonly IUkrainianAnalyzer _analyzer;
        private readonly IResultTrimmer _resultTrimmer;
        private readonly IExceptionDictionary _exceptionDictionary;

        public UkrainianAnalyzerController(
            IUkrainianAnalyzer analyzer,
            IResultTrimmer resultTrimmer,
            IExceptionDictionary exceptionDictionary)
        {
            _analyzer = analyzer;
            _resultTrimmer = resultTrimmer;
            _exceptionDictionary = exceptionDictionary;
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

            if (this.Request.GetQueryString("flags") != null && flags == null)
            {
                throw new InvalidFlagsException();
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
        public HttpResponseMessage Spell(decimal n, string unit, ResponseFormat? format = null)
        {
            if (string.IsNullOrWhiteSpace(unit))
            {
                throw new RequiredParameterIsNotSpecifiedException(nameof(unit));
            }

            NumberSpelling numberSpelling =
                    _analyzer.Spell(n, unit);
            return Request.CreateResponse(HttpStatusCode.OK, numberSpelling, format);
        }

        [Route("userdict")]
        [HttpDelete]
        public HttpResponseMessage UserDictDelete(string s, ResponseFormat? format = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new RequiredParameterIsNotSpecifiedException(nameof(s));
            }

            bool found = _exceptionDictionary.Remove(s);
            return Request.CreateResponse(HttpStatusCode.OK, found, format);
        }

        [Route("userdict")]
        [HttpPost]
        public HttpResponseMessage UserDictAdd([FromBody] CorrectionPostModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Н))
            {
                throw new RequiredParameterIsNotSpecifiedException("Н");
            }

            _exceptionDictionary.Add(model);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("userdict")]
        [HttpGet]
        public HttpResponseMessage UserDictGetAll(ResponseFormat? format = null)
        {
            var result = _exceptionDictionary.GetAll();

            return Request.CreateResponse(HttpStatusCode.OK, result, format);
        }
    }
}