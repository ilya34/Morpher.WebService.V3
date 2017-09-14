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
    using Data.Services;

    public class ServiceController : ApiController
    {
        private readonly IApiThrottler _apiThrottler;
        private readonly IMorpherLog _log;
        private readonly IMorpherDatabase _morpherDatabase;
        private readonly IMorpherCache _morpherCache;

        public ServiceController(IApiThrottler apiThrottler, IMorpherLog log, IMorpherDatabase morpherDatabase, IMorpherCache morpherCache)
        {
            this._apiThrottler = apiThrottler;
            this._log = log;
            _morpherDatabase = morpherDatabase;
            _morpherCache = morpherCache;
        }


        [Route("get_queries_left_for_today")]
        [HttpGet]
        public HttpResponseMessage QueriesLeftToday(ResponseFormat? format = null)
        {
            var guid = Request.GetToken();

            MorpherCacheObject cacheObject = null;
            if (guid == null)
            {
                cacheObject = _apiThrottler.GetQueryLimit(Request.GetClientIp());

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
                cacheObject = _apiThrottler.GetQueryLimit(guid.Value);

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

            bool r = _apiThrottler.RemoveFromCache(postModel.ClientToken.ToLowerInvariant()) != null;
            new Task(() => _log.Sync()).Start();
            new Task(() => _morpherDatabase.UploadMorpherCache(_morpherCache.GetAll())).Start();

            return Request.CreateResponse(
                HttpStatusCode.OK,
                r,
                postModel.Format);
        }
    }
}
