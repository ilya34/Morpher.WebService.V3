namespace Morpher.WebService.V3.Controllers
{
    using System;
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
            try
            {
                Guid? guid = this.Request.GetToken();

                if (entity?.Corrections == null)
                {
                    throw new ModelNotValid("Неверный формат модели");
                }

                entity.Language = "UK";
                entity.NominativeForm = entity.NominativeForm?.ToUpperInvariant();

                this.correction.NewCorrection(entity, guid);

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
    }
}