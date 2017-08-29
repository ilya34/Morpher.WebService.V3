namespace Morpher.WebService.V3.General
{
    using System;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Url = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~/");
            return View("~/General/Views/Home/Index.cshtml");
        }
    }
}