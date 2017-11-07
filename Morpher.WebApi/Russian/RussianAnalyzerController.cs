namespace Morpher.WebService.V3.Russian
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Data;
    using General.Data;
    using General.Data.Exceptions;

    [RoutePrefix("russian")]
    public class RussianAnalyzerController : ApiController
    {
        private readonly IRussianAnalyzer _analyzer;
        private readonly IResultTrimmer _resultTrimmer;
        private readonly IExceptionDictionary _exceptionDictionary;

        public RussianAnalyzerController(
            IRussianAnalyzer analyzer,
            IResultTrimmer resultTrimmer,
            IExceptionDictionary exceptionDictionary)
        {
            _analyzer = analyzer;
            _resultTrimmer = resultTrimmer;
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

            if (this.Request.GetQueryString("flags") != null && flags == null)
            {
                throw new InvalidFlagsException();
            }

            Data.DeclensionResult declensionResult =
                _analyzer.Declension(s, flags);

            _resultTrimmer.Trim(declensionResult, Request.GetToken());

            return Request.CreateResponse(HttpStatusCode.OK, declensionResult, format);
        }

        [Route("declension_list")]
        [ThrottleThis(1, TarificationMode.PerWord)]
        [HttpPost]
        public HttpResponseMessage DeclensionList(
            [FromBody]string text,
            [FromUri]General.Data.DeclensionFlags? flags = null,
            [FromUri]ResponseFormat? format = null)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new RequiredParameterIsNotSpecifiedException(nameof(text));

            var words = text.Split('\n');
            
            Func<string, General.Data.DeclensionFlags?, Data.DeclensionResult> inflector =
                (s, f) =>
            {
                var result = _analyzer.Declension(s, f);
                _resultTrimmer.Trim(result, Request.GetToken());
                return result;
            };

            return format == ResponseFormat.Json ?
            Request.CreateResponse(
                HttpStatusCode.OK,
                DeclensionListResultJson.InflectList(inflector, words, flags),
                ResponseFormat.Json) : 
            Request.CreateResponse(
                HttpStatusCode.OK,
                DeclensionListResultXml.InflectList(inflector, words, flags),
                ResponseFormat.Xml);
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

            var s = _analyzer.Spell(n, unit);

             _resultTrimmer.Trim(s, Request.GetToken());

            return Request.CreateResponse(HttpStatusCode.OK, s, format);
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

        [Route("addstressmarks")]
        [ThrottleThis(100, TarificationMode.PerSymbol)]
        [LogThis]
        [HttpPost]
        public HttpResponseMessage Accentizer([FromBody]string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new RequiredParameterIsNotSpecifiedException(nameof(text));
            }

            string accentized = _analyzer.Accentizer(text);
            return Request.CreateResponse(HttpStatusCode.OK, accentized, WebApiConfig.PlainTextFormatter);
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
            if (string.IsNullOrWhiteSpace(model.И))
            {
                throw new RequiredParameterIsNotSpecifiedException("И");
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