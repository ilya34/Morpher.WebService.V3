namespace Morpher.WebService.V3.Controllers
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Models.Exceptions;
    using Morpher.WebService.V3.Extensions;
    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;

    [RoutePrefix("ukrainian")]
    public class UkrainianAnalyzerController : ApiController
    {
        private readonly IUkrainianAnalyzer analyzer;

        private readonly IApiThrottler apiThrottler;

        private readonly IMorpherLog morpherLog;


        private readonly bool isLocalService;

        public UkrainianAnalyzerController(
            IUkrainianAnalyzer analyzer,
            IApiThrottler apiThrottler,
            IMorpherLog morpherLog)
        {
            this.analyzer = analyzer;
            this.apiThrottler = apiThrottler;
            this.morpherLog = morpherLog;
            this.isLocalService = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]);
        }

        [Route("declension", Name = "UkrainianDeclension")]
        [HttpGet]
        public HttpResponseMessage Declension(string s, DeclensionFlags? flags = null, ResponseFormat? format = null)
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

            //    UkrainianDeclensionResult declensionResult =
            //        this.analyzer.Declension(s, this.Request.GetToken(), flags, paidUser);

            //    this.morpherLog.Log(this.Request);
            //    return this.Request.CreateResponse(HttpStatusCode.OK, declensionResult, format);
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

        [Route("spell")]
        [HttpGet]
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

            //    UkrainianNumberSpelling numberSpelling =
            //        this.analyzer.Spell(n, unit);

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

        [Route("set_correction")]
        [HttpPost]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public HttpResponseMessage AddOrUpdateUserCorrection(
            string н,
            string р = null,
            string д = null,
            string з = null,
            string о = null,
            string м = null,
            string к = null,
            string н_м = null,
            string р_м = null,
            string д_м = null,
            string з_м = null,
            string о_м = null,
            string м_м = null,
            string к_м = null,
            ResponseFormat? format = null)
        {
            throw new NotImplementedException();
            //if (!this.isLocalService)
            //{
            //    return this.Request.CreateResponse(HttpStatusCode.Forbidden, false, format);
            //}

            //if (string.IsNullOrWhiteSpace(н))
            //{
            //    return this.Request.CreateResponse(
            //        HttpStatusCode.BadRequest,
            //        new ServiceErrorMessage(new RequiredParameterIsNotSpecified(nameof(н))),
            //        format);
            //}

            //UkrainianEntry ukrainianEntry = new UkrainianEntry()
            //{
            //    Singular = new UkrainianDeclensionForms()
            //    {
            //        Nominative = н,
            //        Dative = д,
            //        Genitive = р,
            //        Instrumental = о,
            //        Accusative = з,
            //        Prepositional = м,
            //        Vocative = к
            //    },
            //    Plural = new UkrainianDeclensionForms()
            //    {
            //        Nominative = н_м,
            //        Dative = д_м,
            //        Genitive = р_м,
            //        Instrumental = о_м,
            //        Accusative = з_м,
            //        Prepositional = м_м,
            //        Vocative = к_м
            //    }
            //};

            //this.ukrainianDictService.AddOrUpdate(ukrainianEntry);

            //return this.Request.CreateResponse(HttpStatusCode.OK, true, format);
        }

        [Route("remove_correction")]
        [HttpPost]
        public HttpResponseMessage RemoveCorrection(string s, ResponseFormat? format = null)
        {
            throw new NotImplementedException();
            //if (!this.isLocalService)
            //{
            //    return this.Request.CreateResponse(HttpStatusCode.Forbidden, false, format);
            //}

            //this.ukrainianDictService.Remove(s);
            //return this.Request.CreateResponse(HttpStatusCode.Forbidden, true, format);
        }

        [Route("get_all_corrections")]
        [HttpGet]
        public HttpResponseMessage GetAllCorrections(ResponseFormat? format = null)
        {
            throw new NotImplementedException();
            //if (!this.isLocalService)
            //{
            //    return this.Request.CreateResponse(HttpStatusCode.Forbidden, false, format);
            //}

            //return this.Request.CreateResponse(HttpStatusCode.OK, this.ukrainianDictService.GetAll(), format);
        }
    }
}