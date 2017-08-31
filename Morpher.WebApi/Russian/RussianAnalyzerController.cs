namespace Morpher.WebService.V3.Russian
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Data;
    using General.Data;

    [RoutePrefix("russian")]
    public class RussianAnalyzerController : ApiController
    {
        private readonly IRussianAnalyzer _analyzer;
        private readonly IResultTrimmer _resultTrimmer;
        private readonly IUserDictionaryLookup _dictionaryLookup;
        private readonly IExceptionDictionary _exceptionDictionary;

        public RussianAnalyzerController(
            IRussianAnalyzer analyzer,
            IResultTrimmer resultTrimmer,
            IUserDictionaryLookup dictionaryLookup,
            IExceptionDictionary exceptionDictionary)
        {
            _analyzer = analyzer;
            _resultTrimmer = resultTrimmer;
            _dictionaryLookup = dictionaryLookup;
            _exceptionDictionary = exceptionDictionary;
        }

        [Route("declension", Name = "RussianDeclension")]
        [ThrottleThis]
        [LogThis]
        [HttpGet]
        public HttpResponseMessage Declension(string s, General.Data.DeclensionFlags? flags = null, ResponseFormat? format = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new RequiredParameterIsNotSpecifiedException(nameof(s));
            }

            Data.DeclensionResult declensionResult =
                _analyzer.Declension(s, flags);

            var corrections = _dictionaryLookup.Lookup(s);

            if (corrections != null)
            {
                declensionResult = new ExceptionResult(corrections, declensionResult);
            }

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
                throw new RequiredParameterIsNotSpecifiedException(nameof(s));
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
                throw new RequiredParameterIsNotSpecifiedException(nameof(s));
            }

            Data.AdjectiveGenders adjectives = _analyzer.AdjectiveGenders(s);
            return Request.CreateResponse(HttpStatusCode.OK, adjectives, format);
        }

        [Route("userdict")]
        [HttpDelete]
        public HttpResponseMessage UserDictDelete()
        {
            throw new NotImplementedException();
        }

        [Route("userdict")]
        [HttpPost]
        public HttpResponseMessage UserDictAdd([FromBody] CorrectionPostModel model)
        {
            if (model.IsEmpty())
            {
                throw new Exception("Исключение сделай");
            }


            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("userdict")]
        [ThrottleThis]
        [HttpGet]
        public HttpResponseMessage UserDictGetAll(ResponseFormat? format = null)
        {
            var result = _exceptionDictionary.GetAll();

            return Request.CreateResponse(HttpStatusCode.NotFound, result, format);
        }
    }
}