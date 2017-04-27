namespace Morpher.WebApi.Controllers
{
    using System;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Default()
        {
            ViewBag.Title = "Примеры вызова";
            ViewBag.Url = "http://api.morphger.ru/";
            return this.View();
        }
    }
}