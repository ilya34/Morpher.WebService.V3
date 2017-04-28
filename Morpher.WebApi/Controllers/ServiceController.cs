namespace Morpher.WebApi.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Morpher.WebApi.Extensions;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Exceptions;
    using Morpher.WebApi.Services.Interfaces;

    public class ServiceController : ApiController
    {
        private readonly IApiThrottler apiThrottler;

        public ServiceController(IApiThrottler apiThrottler)
        {
            this.apiThrottler = apiThrottler;
        }

        [Route("daily_query_limit")]
        [HttpGet]
        public HttpResponseMessage GetQueryLimit(string token = null, ResponseFormat? format = null)
        {
            Guid? guid = this.Request.GetToken();

            var cacheObject = guid == null ? 
                                          this.apiThrottler.GetQueryLimit(this.Request.GetClientIp()) : 
                                          this.apiThrottler.GetQueryLimit(guid.Value);

            if (cacheObject != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, cacheObject.DailyLimit, format);
            }

            return this.Request.CreateResponse(
                HttpStatusCode.OK,
                new ServiceErrorMessage(new TokenNotFoundException()),
                format);
        }

        [Route("validate_client_token")]
        [HttpGet]
        public HttpResponseMessage ValidateClientToken(string clientToken, ResponseFormat? format = null)
        {
            string ip = this.Request.GetClientIp();

            if (ip != "::1")
            {
                return this.Request.CreateResponse(HttpStatusCode.Forbidden, "Not today", format);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, this.apiThrottler.UpdateCache(clientToken), format);
        }
    }
}
