namespace Morpher.WebService.V3.Controllers
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Morpher.WebService.V3.Extensions;
    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Interfaces;
    using Morpher.WebService.V3.Shared.Models;
    using Morpher.WebService.V3.Shared.Models.Exceptions;

    [RoutePrefix("ukrainian")]
    public class UkrainianAnalyzerController : ApiController
    {
        private readonly IUkrainianAnalyzer analyzer;

        private readonly IApiThrottler apiThrottler;

        private readonly IMorpherLog morpherLog;

        private readonly IUserCorrection correction;

        private readonly bool isLocalService;

        public UkrainianAnalyzerController(
            IUkrainianAnalyzer analyzer,
            IApiThrottler apiThrottler,
            IMorpherLog morpherLog,
            IUserCorrection correction)
        {
            this.analyzer = analyzer;
            this.apiThrottler = apiThrottler;
            this.morpherLog = morpherLog;
            this.correction = correction;
            this.isLocalService = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]);
        }

        [Route("declension", Name = "UkrainianDeclension")]
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
                ApiThrottlingResult result = this.apiThrottler.Throttle(this.Request,out paidUser);

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

        [Route("set_correction")]
        [HttpPost]
        public HttpResponseMessage AddOrUpdateUserCorrection(
            [FromBody] UserCorrectionEntity entity,
            ResponseFormat? format = null,
            bool? morpherRequest = false)
        {
            if (!this.isLocalService)
            {
                return this.Request.CreateResponse(HttpStatusCode.Forbidden, false, ResponseFormat.Xml);
            }

            try
            {
                if (entity?.Corrections == null)
                {
                    throw new ModelNotValid("Неверный формат модели");
                }

                entity.Language = "UK";
                entity.NominativeForm = entity.NominativeForm?.ToUpperInvariant();

                this.correction.NewCorrection(entity, null);

                return this.Request.CreateResponse(HttpStatusCode.OK, true, ResponseFormat.Xml);
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

        [Route("remove_correction")]
        [HttpPost]
        public HttpResponseMessage RemoveCorrection(string lemma, ResponseFormat? format = null)
        {
            if (!this.isLocalService)
            {
                return this.Request.CreateResponse(HttpStatusCode.Forbidden, false, ResponseFormat.Xml);
            }

            try
            {
                if (string.IsNullOrWhiteSpace(lemma))
                {
                    throw new RequiredParameterIsNotSpecified(nameof(lemma));
                }

                bool result = this.correction.RemoveCorrection(lemma, "UK", null);

                return this.Request.CreateResponse(HttpStatusCode.OK, result, format);
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