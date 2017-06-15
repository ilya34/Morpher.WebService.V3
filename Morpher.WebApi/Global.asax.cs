namespace Morpher.WebService.V3
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Dapper;

    using FluentScheduler;

    using Morpher.WebService.V3.Database;
    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services;
    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Models;

    using Ninject;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
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

            IKernel kernel = (IKernel)DependencyResolver.Current.GetService(typeof(IKernel));
            IMorpherCache userCorrectCache = kernel.Get<IMorpherCache>("UserCorrection");
            if (isLocal)
            {
                // TODO: Load from file
            }
            else
            {
                this.LoadUserCorrectionsFromDatabase(userCorrectCache);
            }
        }

        private void LoadUserCorrectionsFromDatabase(IMorpherCache morpherCache)
        {
            using (UserCorrectionDataContext context = new UserCorrectionDataContext())
            {
                var userIds = context.UserVotes.GroupBy(vote => vote.UserID).ToList();
                foreach (var userId in userIds)
                {
                    List<UserCorrectionEntity> entities = new List<UserCorrectionEntity>();
                    foreach (var userVote in userId)
                    {
                        UserCorrectionEntity entity = new UserCorrectionEntity();
                        var name = context.Names.Single(name1 => name1.ID == userVote.NameID);
                        entity.NominativeForm = name.Lemma;
                        entity.Language = name.LanguageID;
                        var forms = context.NameForms.Where(form => form.NameID == userVote.NameID);
                        List<Correction> corrections = new List<Correction>();
                        foreach (var nameForm in forms)
                        {
                            corrections.Add(new Correction()
                            {
                                Lemma = nameForm.AccentedText,
                                Form = nameForm.FormID.ToString(),
                                Plural = nameForm.Plural,
                            });
                        }

                        entity.Corrections = corrections;
                        entities.Add(entity);
                    }

                    morpherCache.Set(userId.Key.ToString().ToLowerInvariant(), entities, new CacheItemPolicy());
                }
            }
        }

        protected void Application_End()
        {
            IMorpherLog log = (IMorpherLog)DependencyResolver.Current.GetService(typeof(IMorpherLog));
            log.Sync();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
        }
    }
}