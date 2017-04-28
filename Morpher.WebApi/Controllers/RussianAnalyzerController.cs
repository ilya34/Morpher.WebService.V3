namespace Morpher.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Morpher.WebApi.ApiThrottler;
    using Morpher.WebApi.Extensions;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Exceptions;
    using Morpher.WebApi.Services.Interfaces;

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
        public HttpResponseMessage Declension(string s, string token = null, ResponseFormat? format = null)
        {
            try
            {
                ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request, out bool paidUser);

                if (result != ApiThrottlingResult.Success)
                {
                    throw result.GenerateMorpherException();
                }

                RussianDeclensionResult declensionResult =
                    this.analyzer.Declension(s, this.Request.GetToken(), paidUser);

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
        public HttpResponseMessage Spell(decimal n, string unit, string token = null, ResponseFormat? format = null)
        {
            try
            {
                ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request, out bool paidUser);

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
        public HttpResponseMessage Adjectivize(string s, string token = null, ResponseFormat? format = null)
        {
            try
            {
                ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request, out bool paidUser);

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
        public HttpResponseMessage AdjectiveGenders(string s, string token = null, ResponseFormat? format = null)
        {
            try
            {
                ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request, out bool paidUser);

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
    }
}
