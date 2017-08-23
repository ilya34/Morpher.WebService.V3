namespace Morpher.WebService.V3
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using App_Start;
    using Autofac;
    using Autofac.Integration.WebApi;
    using FluentScheduler;
    using Helpers;
    using Morpher.WebService.V3.Services;
    using Morpher.WebService.V3.Services.Interfaces;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutofacInit.Init();

            var urls = ThrottleUrls.Urls;
            bool isLocal = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]);

            if (!isLocal)
            {
                NameValueCollection conf = (NameValueCollection)ConfigurationManager.GetSection("WebServiceSettings");
                int everyMinutes = Convert.ToInt32(conf["SyncCacheEveryMinutes"]);
                Registry registry = new Registry();
                registry.Schedule<LogSyncer>().ToRunEvery(everyMinutes).Minutes();
                JobManager.Initialize(registry);
            }

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //IKernel kernel = (IKernel)DependencyResolver.Current.GetService(typeof(IKernel));

            //if (isLocal)
            //{
            //    IRussianDictService russianDictService = kernel.Get<IRussianDictService>();
            //    IUkrainianDictService ukrainianDictService = kernel.Get<IUkrainianDictService>();
            //    string filePathRu = Server.MapPath("~/App_Data/UserDict.xml");
            //    if (File.Exists(filePathRu))
            //    {                    
            //        using (StreamReader streamReader = new StreamReader(filePathRu))
            //        {
            //            var list = RussianDictService.LoadFromXml(streamReader);
            //            russianDictService.Load(list);
            //        }
            //    }

            //    string filePathUkr = Server.MapPath("~/App_Data/UserDictUkr.xml");
            //    if (File.Exists(filePathUkr))
            //    {
            //        using (StreamReader streamReader = new StreamReader(filePathUkr))
            //        {
            //            var list = UkrainianDictService.LoadFromXml(streamReader);
            //            ukrainianDictService.Load(list);
            //        }
            //    }
            //}
        }

        protected void Application_End()
        {
            IMorpherLog log = (IMorpherLog)DependencyResolver.Current.GetService(typeof(IMorpherLog));
            log.Sync();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = this.Server.GetLastError();
        }
    }
}