namespace Morpher.WebService.V3.Controllers
{
    using System;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            this.ViewBag.Url = this.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~/");
            return this.View();
        }
    }
}