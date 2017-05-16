namespace Morpher.WebService.V3.Controllers
{
    using System;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var requestUrl = this.HttpContext.Request.Url;
            if (requestUrl != null)
            {
                this.ViewBag.Url = requestUrl.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
            }

            return this.View();
        }
    }
}