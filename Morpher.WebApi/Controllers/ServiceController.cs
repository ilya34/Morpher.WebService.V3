namespace Morpher.WebService.V3.Controllers
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Morpher.WebService.V3.Extensions;
    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Models.Exceptions;
    using Morpher.WebService.V3.Services.Interfaces;

    public class ServiceController : ApiController
    {
        private readonly IApiThrottler apiThrottler;

        private readonly IMorpherLog log;

        public ServiceController(IApiThrottler apiThrottler, IMorpherLog log)
        {
            this.apiThrottler = apiThrottler;
            this.log = log;
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
        public HttpResponseMessage RemoveClientFromCache([FromBody]CacheResetPostModel postModel)
        {
            NameValueCollection conf = (NameValueCollection)ConfigurationManager.GetSection("WebServiceSettings");

            if (postModel.AdminPassword != conf["CacheResetKey"])
            {
                return this.Request.CreateResponse(HttpStatusCode.Forbidden, "Not today", postModel.Format);
            }

            new Task(() => this.log.Sync()).Start();

            return this.Request.CreateResponse(
                HttpStatusCode.OK,
                this.apiThrottler.RemoveFromCache(postModel.ClientToken.ToLowerInvariant()) != null,
                postModel.Format);
        }
    }
}
