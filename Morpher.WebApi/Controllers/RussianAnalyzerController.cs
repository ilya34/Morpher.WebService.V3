namespace Morpher.WebService.V3.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Morpher.WebService.V3.Extensions;
    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Interfaces;
    using Morpher.WebService.V3.Shared.Models;
    using Morpher.WebService.V3.Shared.Models.Exceptions;

    [RoutePrefix("russian")]
    public class RussianAnalyzerController : ApiController
    {
        private readonly IRussianAnalyzer analyzer;

        private readonly IApiThrottler apiThrottler;

        private readonly IMorpherLog morpherLog;

        public RussianAnalyzerController(IRussianAnalyzer analyzer, IApiThrottler apiThrottler, IMorpherLog morpherLog)
        {
            this.analyzer = analyzer;
            this.apiThrottler = apiThrottler;
            this.morpherLog = morpherLog;
        }

        [Route("declension", Name = "RussianDeclension")]
        [HttpGet]
        public HttpResponseMessage Declension(string s, DeclensionFlags? flags = null, ResponseFormat? format = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    throw new RequiredParameterIsNotSpecified(nameof(s));
                }

                bool paidUser;
                ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request, out paidUser);

                if (result != ApiThrottlingResult.Success)
                {
                    throw result.GenerateMorpherException();
                }

                RussianDeclensionResult declensionResult =
                    this.analyzer.Declension(s, this.Request.GetToken(), flags, paidUser);

                this.morpherLog.Log(this.Request);
                return this.Request.CreateResponse(HttpStatusCode.OK, declensionResult, format);
            }
            catch (MorpherException exception)
            {
                this.morpherLog.Log(this.Request, exception);
                return this.Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    new ServiceErrorMessage(exception),
                    format);
            }
        }

        [Route("spell")]
        [HttpGet]
        public HttpResponseMessage Spell(int n, string unit, ResponseFormat? format = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(unit))
                {
                    throw new RequiredParameterIsNotSpecified(nameof(unit));
                }

                bool paidUser;
                ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request, out paidUser);

                if (result != ApiThrottlingResult.Success)
                {
                    throw result.GenerateMorpherException();
                }

                RussianNumberSpelling numberSpelling =
                    this.analyzer.Spell(n, unit);

                this.morpherLog.Log(this.Request);
                return this.Request.CreateResponse(HttpStatusCode.OK, numberSpelling, format);
            }
            catch (MorpherException exception)
            {
                this.morpherLog.Log(this.Request, exception);
                return this.Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    new ServiceErrorMessage(exception),
                    format);
            }
        }

        [Route("adjectivize")]
        [HttpGet]
        public HttpResponseMessage Adjectivize(string s, ResponseFormat? format = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    throw new RequiredParameterIsNotSpecified(nameof(s));
                }

                bool paidUser;
                ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request, out paidUser);

                if (result != ApiThrottlingResult.Success)
                {
                    throw result.GenerateMorpherException();
                }

                List<string> adjectives =
                    this.analyzer.Adjectives(s);

                this.morpherLog.Log(this.Request);
                return this.Request.CreateResponse(HttpStatusCode.OK, adjectives, format);
            }
            catch (MorpherException exception)
            {
                this.morpherLog.Log(this.Request, exception);
                return this.Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    new ServiceErrorMessage(exception),
                    format);
            }
        }

        [Route("genders")]
        [HttpGet]
        public HttpResponseMessage AdjectiveGenders(string s, ResponseFormat? format = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    throw new RequiredParameterIsNotSpecified(nameof(s));
                }

                bool paidUser;
                ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request, out paidUser);

                if (result != ApiThrottlingResult.Success)
                {
                    throw result.GenerateMorpherException();
                }

                AdjectiveGenders adjectives =
                    this.analyzer.AdjectiveGenders(s);

                this.morpherLog.Log(this.Request);
                return this.Request.CreateResponse(HttpStatusCode.OK, adjectives, format);
            }
            catch (MorpherException exception)
            {
                this.morpherLog.Log(this.Request, exception);
                return this.Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    new ServiceErrorMessage(exception),
                    format);
            }
        }

        [Route("set_correction")]
        [HttpPost]
        public HttpResponseMessage SetCorrection([FromBody] UserCorrectionPostModel postModel, ResponseFormat? format = null)
        {
            Guid? guid = this.Request.GetToken();

            if (guid != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.Forbidden, 
                    new MorpherException(), format);
            }

            try
            {
            }
            catch (MorpherException)
            {
                
            }
            

            return this.Request.CreateResponse(HttpStatusCode.OK, postModel, ResponseFormat.Xml);
        }
    }
}
