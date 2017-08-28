namespace Morpher.WebService.V3.App_Start
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Compilation;
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Autofac.Integration.WebApi;
    using Filters;
    using Helpers;
    using Middlewares;
    using Services;
    using Services.Interfaces;

    public static class AutofacInit
    {
        public static IContainer Container { get; private set; }

        public static AutofacWebApiDependencyResolver AutofacWebApiDependencyResolver { get; private set; }

        public static void Init()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterWebApiModelBinderProvider();

            RegisterServices(builder);

            Container = builder.Build();
            AutofacWebApiDependencyResolver = new AutofacWebApiDependencyResolver(Container);
            config.DependencyResolver = AutofacWebApiDependencyResolver;
            
            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }

        private static void RegisterServices(ContainerBuilder builder)
        {

            bool runAsLocalService = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("RunAsLocalService"));


            if (runAsLocalService)
            {
                RegisterLocalOnlyServices(builder);
            }
            else
            {
                RegisterGlobalOnlyServices(builder);
            }

            RegisterSharedServices(builder);
            RegisterAnalyzers(builder);
        }

        private static void RegisterAnalyzers(ContainerBuilder builder)
        {
            string externalAnalyzer = ConfigurationManager.AppSettings.Get("ExternalAnalyzer");

            if (externalAnalyzer == null)
            {
                MorpherClient client = new MorpherClient();
                builder.RegisterType<RussianWebAnalyzer>().As<IRussianAnalyzer>()
                    .WithParameter("client", client.Russian)
                    .SingleInstance();
                builder.RegisterType<UkrainianWebAnalyzer>().As<IUkrainianAnalyzer>()
                    .WithParameter("client", client.Ukrainian)
                    .SingleInstance();
            }
            else
            {
                string path = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "bin",
                    externalAnalyzer);
                var analyzer = Assembly.LoadFile(path);

                builder.RegisterAssemblyTypes(analyzer)
                    .Where(type => typeof(IRussianAnalyzer).IsAssignableFrom(type))
                    .As<IRussianAnalyzer>();
                builder.RegisterAssemblyTypes(analyzer)
                    .Where(type => typeof(IUkrainianAnalyzer).IsAssignableFrom(type))
                    .As<IUkrainianAnalyzer>();
            }
        }

        private static void RegisterSharedServices(ContainerBuilder builder)
        {
            builder.RegisterType<MorpherCache>()
                .As<IMorpherCache>()
                .WithParameter("name", "ApiThrottler")
                .SingleInstance();

            // Filters
            builder.Register(context => new MorpherExceptionFilterAttribute())
                .AsWebApiExceptionFilterFor<ApiController>().SingleInstance();
        }

        private static void RegisterLocalOnlyServices(ContainerBuilder builder)
        {
            builder.RegisterType<GuidInsertMiddleware>();
        }

        private static void RegisterGlobalOnlyServices(ContainerBuilder builder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MorpherDatabase"].ConnectionString;

            // Используются в middlewares для трэкинга нужных URL
            builder.RegisterType<AttributeUrls>()
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("ApiThrottler")
                .SingleInstance()
                .WithParameter("attributeType", typeof(ThrottleThisAttribute));
            builder.RegisterType<AttributeUrls>()
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("Logger")
                .SingleInstance()
                .WithParameter("attributeType", typeof(LogThisAttribute));

            builder.RegisterType<LogSyncer>().AsSelf().InstancePerLifetimeScope();

            // Используется в LoggingMiddleware
            builder.RegisterType<MorpherLog>().As<IMorpherLog>().SingleInstance();
            builder.RegisterType<DatabaseLog>().As<IDatabaseLog>()
                .WithParameter("connectionString", connectionString);

            // Исплользуется в ThrottlingMIddleware
            builder.RegisterType<ApiThrottler>().As<IApiThrottler>();
            builder.RegisterType<MorpherDatabase>().As<IMorpherDatabase>()
                .WithParameter("connectionString", connectionString);

            // Middlewares
            builder.RegisterType<ThrottlingMiddleware>();
            builder.RegisterType<LoggingMiddleware>();
        }
    }
}