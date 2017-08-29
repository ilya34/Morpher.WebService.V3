namespace Morpher.WebService.V3.General
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Data;

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
            var guid = Request.GetToken();

            MorpherCacheObject cacheObject = null;
            if (guid == null)
            {
                cacheObject = apiThrottler.GetQueryLimit(Request.GetClientIp());

                if (cacheObject == null)
                {
                    return Request.CreateResponse(
                        HttpStatusCode.OK,
                        new ServiceErrorMessage(new IpBlockedExceptionException()),
                        format);
                }
            }
            else
            {
                cacheObject = apiThrottler.GetQueryLimit(guid.Value);

                if (cacheObject == null)
                {
                    return Request.CreateResponse(
                        HttpStatusCode.OK,
                        new ServiceErrorMessage(new TokenNotFoundExceptionException()),
                        format);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, Math.Max(cacheObject.QueriesLeft, 0), format);
        }

        [Route("remove_client_from_cache")]
        [HttpPost]
        public HttpResponseMessage RemoveClientFromCache([FromBody]CacheResetPostModel postModel)
        {
            NameValueCollection conf = (NameValueCollection)ConfigurationManager.GetSection("WebServiceSettings");

            if (postModel.AdminPassword != conf["CacheResetKey"])
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Not today", postModel.Format);
            }

            new Task(() => log.Sync()).Start();

            return Request.CreateResponse(
                HttpStatusCode.OK,
                apiThrottler.RemoveFromCache(postModel.ClientToken.ToLowerInvariant()) != null,
                postModel.Format);
        }
    }
}
