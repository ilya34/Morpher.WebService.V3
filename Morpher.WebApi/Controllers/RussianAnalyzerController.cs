namespace Morpher.WebService.V3.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Helpers;
    using Models.Exceptions;
    using Extensions;
    using Models;
    using Services.Interfaces;

    [RoutePrefix("russian")]
    public class RussianAnalyzerController : ApiController
    {
        private readonly IRussianAnalyzer _analyzer;
        private readonly IResultTrimmer _resultTrimmer;

        public RussianAnalyzerController(
            IRussianAnalyzer analyzer,
            IResultTrimmer resultTrimmer)
        {
            _analyzer = analyzer;
            _resultTrimmer = resultTrimmer;
        }

        [Route("declension", Name = "RussianDeclension")]
        [ThrottleThis]
        [LogThis]
        [HttpGet]
        public HttpResponseMessage Declension(string s, DeclensionFlags? flags = null, ResponseFormat? format = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new RequiredParameterIsNotSpecified(nameof(s));
            }

            RussianDeclensionResult declensionResult =
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
                throw new RequiredParameterIsNotSpecified(nameof(unit));
            }

            return Request.CreateResponse(HttpStatusCode.OK, _analyzer.Spell(n, unit), format);
        }

        [Route("adjectivize")]
        [ThrottleThis]
        [LogThis]
        [HttpGet]
        public HttpResponseMessage Adjectivize(string s, ResponseFormat? format = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new RequiredParameterIsNotSpecified(nameof(s));
            }

            List<string> adjectives = _analyzer.Adjectives(s);
            return Request.CreateResponse(HttpStatusCode.OK, adjectives, format);
        }

        [Route("genders")]
        [ThrottleThis]
        [LogThis]
        [HttpGet]
        public HttpResponseMessage AdjectiveGenders(string s, ResponseFormat? format = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new RequiredParameterIsNotSpecified(nameof(s));
            }

            AdjectiveGenders adjectives = _analyzer.AdjectiveGenders(s);
            return Request.CreateResponse(HttpStatusCode.OK, adjectives, format);
        }
    }
}