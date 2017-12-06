using System.Net;
using System.Net.Http;
using System.Web.Http;
using Morpher.WebService.V3.General.Data;

namespace Morpher.WebService.V3.Qazaq
{
    [RoutePrefix("Qazaq")]
    public class QazaqAnalyzerController : ApiController
    {
        private readonly IQazaqAnalyzer _analyzer;

        public QazaqAnalyzerController(IQazaqAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        [Route("declension")]
        [ThrottleThis]
        [LogThis]
        [HttpGet]
        public HttpResponseMessage Declension(string s, ResponseFormat? format = null)
        {
            if (string.IsNullOrEmpty(s)) throw new RequiredParameterIsNotSpecifiedException(nameof(s));

            var declensionResult = _analyzer.Declension(s);

            return Request.CreateResponse(HttpStatusCode.OK, declensionResult, format);
        }
    }
}
