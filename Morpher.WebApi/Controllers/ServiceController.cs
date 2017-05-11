namespace Morpher.WebApi.Controllers
{
    using System;
    using System.Configuration;
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

        [Route("get_queries_left_for_today")]
        [HttpGet]
        public HttpResponseMessage QueriesLeftToday(ResponseFormat? format = null)
        {
            Guid? guid;
            try
            {
                guid = this.Request.GetToken();
            }
            catch (MorpherException exception)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, new ServiceErrorMessage(exception), format);
            }

            MorpherCacheObject cacheObject = null;
            if (guid == null)
            {
                cacheObject = this.apiThrottler.GetQueryLimit(this.Request.GetClientIp());

                if (cacheObject == null)
                {
                    return this.Request.CreateResponse(
                        HttpStatusCode.OK,
                        new ServiceErrorMessage(new IpBlockedException()),
                        format);
                }
            }
            else
            {
                cacheObject = this.apiThrottler.GetQueryLimit(guid.Value);

                if (cacheObject == null)
                {
                    return this.Request.CreateResponse(
                        HttpStatusCode.OK,
                        new ServiceErrorMessage(new TokenNotFoundException()),
                        format);
                }
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, Math.Max(cacheObject.QueriesLeft, 0), format);
        }

        [Route("remove_client_from_cache")]
        [HttpPost]
        public HttpResponseMessage RemoveClientFromCache(string clientToken, string password, ResponseFormat? format = null)
        {
            if (password != ConfigurationManager.AppSettings["CacheResetKey"])
            {
                return this.Request.CreateResponse(HttpStatusCode.Forbidden, "Not today", format);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, this.apiThrottler.RemoveFromCache(clientToken.ToLowerInvariant()) != null, format);
        }
    }
}
