﻿namespace Morpher.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Morpher.WebApi.ApiThrottler;
    using Morpher.WebApi.Extensions;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Exceptions;
    using Morpher.WebApi.Services.Interfaces;

    [RoutePrefix("ukrainian")]
    public class UkrainianAnalyzerController : ApiController
    {
        private readonly IUkrainianAnalyzer analyzer;

        private readonly IApiThrottler apiThrottler;

        private readonly IMorpherLog morpherLog;

        public UkrainianAnalyzerController(IUkrainianAnalyzer analyzer, IApiThrottler apiThrottler, IMorpherLog morpherLog)
        {
            this.analyzer = analyzer;
            this.apiThrottler = apiThrottler;
            this.morpherLog = morpherLog;
        }

        [Route("declension", Name = "UkrainianDeclension")]
        [HttpGet]
        public HttpResponseMessage Declension(string s, string token = null, DeclensionFlags? flags = null, ResponseFormat? format = null)
        {
            try
            {
                ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request, out bool paidUser);

                if (result != ApiThrottlingResult.Success)
                {
                    throw result.GenerateMorpherException();
                }

                UkrainianDeclensionResult declensionResult =
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
        public HttpResponseMessage Spell(int n, string unit, string token = null, ResponseFormat? format = null)
        {
            try
            {
                ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request, out bool paidUser);

                if (result != ApiThrottlingResult.Success)
                {
                    throw result.GenerateMorpherException();
                }

                UkrainianNumberSpelling numberSpelling =
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
    }
}