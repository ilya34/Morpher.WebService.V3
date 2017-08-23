namespace Morpher.WebService.V3.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Helpers;
    using Models.Exceptions;
    using Morpher.WebService.V3.Extensions;
    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;

    [RoutePrefix("russian")]
    public class RussianAnalyzerController : ApiController
    {
        private readonly IRussianAnalyzer _analyzer;

        public RussianAnalyzerController(
            IRussianAnalyzer analyzer)
        {
            this._analyzer = analyzer;
        }

        public bool IsLocalService { get; set; }

        [Route("declension", Name = "RussianDeclension")]
        [HttpGet]
        [ThrottleThis]
        public HttpResponseMessage Declension(string s, DeclensionFlags? flags = null, ResponseFormat? format = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    throw new RequiredParameterIsNotSpecified(nameof(s));
                }

                RussianDeclensionResult declensionResult =
                    _analyzer.Declension(s, flags);

                return this.Request.CreateResponse(HttpStatusCode.OK, declensionResult, format);
            }
            catch (MorpherException exception)
            {
                return this.Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    new ServiceErrorMessage(exception),
                    format);
            }
        }

        [Route("spell")]
        [HttpGet]
        [ThrottleThis]
        public HttpResponseMessage Spell(int n, string unit, ResponseFormat? format = null)
        {
            throw new NotImplementedException();
            //try
            //{
            //    if (string.IsNullOrWhiteSpace(unit))
            //    {
            //        throw new RequiredParameterIsNotSpecified(nameof(unit));
            //    }

            //    bool paidUser;
            //    ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request, out paidUser);

            //    if (result != ApiThrottlingResult.Success)
            //    {
            //        throw result.GenerateMorpherException();
            //    }

            //    RussianNumberSpelling numberSpelling = this.analyzer.Spell(n, unit);

            //    this.morpherLog.Log(this.Request);
            //    return this.Request.CreateResponse(HttpStatusCode.OK, numberSpelling, format);
            //}
            //catch (MorpherException exception)
            //{
            //    this.morpherLog.Log(this.Request, exception);
            //    return this.Request.CreateResponse(
            //        HttpStatusCode.BadRequest,
            //        new ServiceErrorMessage(exception),
            //        format);
            //}
        }

        [Route("adjectivize")]
        [HttpGet]
        [ThrottleThis]
        public HttpResponseMessage Adjectivize(string s, ResponseFormat? format = null)
        {
            throw new NotImplementedException();
            //try
            //{
            //    if (string.IsNullOrWhiteSpace(s))
            //    {
            //        throw new RequiredParameterIsNotSpecified(nameof(s));
            //    }

            //    bool paidUser;
            //    ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request, out paidUser);

            //    if (result != ApiThrottlingResult.Success)
            //    {
            //        throw result.GenerateMorpherException();
            //    }

            //    List<string> adjectives = this.analyzer.Adjectives(s);

            //    this.morpherLog.Log(this.Request);
            //    return this.Request.CreateResponse(HttpStatusCode.OK, adjectives, format);
            //}
            //catch (MorpherException exception)
            //{
            //    this.morpherLog.Log(this.Request, exception);
            //    return this.Request.CreateResponse(
            //        HttpStatusCode.BadRequest,
            //        new ServiceErrorMessage(exception),
            //        format);
            //}
        }

        [Route("genders")]
        [HttpGet]
        [ThrottleThis]
        public HttpResponseMessage AdjectiveGenders(string s, ResponseFormat? format = null)
        {
            throw new NotImplementedException();
            //try
            //{
            //    if (string.IsNullOrWhiteSpace(s))
            //    {
            //        throw new RequiredParameterIsNotSpecified(nameof(s));
            //    }

            //    bool paidUser;
            //    ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request, out paidUser);

            //    if (result != ApiThrottlingResult.Success)
            //    {
            //        throw result.GenerateMorpherException();
            //    }

            //    AdjectiveGenders adjectives = this.analyzer.AdjectiveGenders(s);

            //    this.morpherLog.Log(this.Request);
            //    return this.Request.CreateResponse(HttpStatusCode.OK, adjectives, format);
            //}
            //catch (MorpherException exception)
            //{
            //    this.morpherLog.Log(this.Request, exception);
            //    return this.Request.CreateResponse(
            //        HttpStatusCode.BadRequest,
            //        new ServiceErrorMessage(exception),
            //        format);
            //}
        }


        [Route("userdict")]
        [HttpPost]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public HttpResponseMessage AddOrUpdateUserCorrection(
            string и,
            string р = null,
            string д = null,
            string в = null,
            string т = null,
            string п = null,
            string п_о = null,
            string и_м = null,
            string р_м = null,
            string д_м = null,
            string в_м = null,
            string т_м = null,
            string п_м = null,
            string п_о_м = null,
            ResponseFormat? format = null)
        {
            throw new NotImplementedException();
            //if (!this.IsLocalService)
            //{
            //    return this.Request.CreateResponse(HttpStatusCode.Forbidden, false, format);
            //}

            //if (string.IsNullOrWhiteSpace(и))
            //{
            //    return this.Request.CreateResponse(
            //        HttpStatusCode.BadRequest,
            //        new ServiceErrorMessage(new RequiredParameterIsNotSpecified(nameof(и))),
            //        format);
            //}

            //RussianEntry russianEntry = new RussianEntry()
            //{
            //    Singular = new RussianDeclensionForms()
            //    {
            //        Nominative = и,
            //        Dative = д,
            //        Genitive = р,
            //        Instrumental = т,
            //        Accusative = в,
            //        Prepositional = п,
            //        PrepositionalWithPre = п_о
            //    },
            //    Plural = new RussianDeclensionForms()
            //    {
            //        Nominative = и_м,
            //        Dative = д_м,
            //        Genitive = р_м,
            //        Instrumental = т_м,
            //        Accusative = в_м,
            //        Prepositional = п_м,
            //        PrepositionalWithPre = п_о_м
            //    }
            //};

            //this.russianDictService.AddOrUpdate(russianEntry);

            //return this.Request.CreateResponse(HttpStatusCode.OK, true, format);
        }

        [Route("userdict")]
        [HttpDelete]
        public HttpResponseMessage RemoveCorrection(string s, ResponseFormat? format = null)
        {
            throw new NotImplementedException();
            //if (!this.IsLocalService)
            //{
            //    return this.Request.CreateResponse(HttpStatusCode.Forbidden, false, format);
            //}

            //this.russianDictService.Remove(s);
            //return this.Request.CreateResponse(HttpStatusCode.Forbidden, true, format);
        }

        [Route("userdict")]
        [HttpGet]
        public HttpResponseMessage GetAllCorrections(ResponseFormat? format = null)
        {
            throw new NotImplementedException();
            //if (!this.IsLocalService)
            //{
            //    return this.Request.CreateResponse(HttpStatusCode.Forbidden, false, format);
            //}

            //return this.Request.CreateResponse(HttpStatusCode.OK, this.russianDictService.GetAll(), format);
        }
    }
}