namespace Morpher.WebApi.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Morpher.WebApi.Extensions;
    using Morpher.WebApi.Models;
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

            return this.Request.CreateResponse(HttpStatusCode.OK, cacheObject.DailyLimit, format);
        }
    }
}
