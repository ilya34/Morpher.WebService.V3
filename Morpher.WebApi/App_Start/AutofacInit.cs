namespace Morpher.WebService.V3
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Web.Http;
    using Autofac;
    using Autofac.Core;
    using Autofac.Integration.Mvc;
    using Autofac.Integration.WebApi;
    using General.Data;
    using General.Data.Middlewares;
    using General.Data.Services;
    using Russian;
    using Ukrainian;

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
            builder.RegisterType<DummyResultTrimmer>().As<IResultTrimmer>();
        }

        private static void RegisterGlobalOnlyServices(ContainerBuilder builder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MorpherDatabase"].ConnectionString;

            // Используются в middlewares для трэкинга нужных URL
            builder.RegisterType<AttributeUrls>()
                .As<IAttributeUrls>()
                .SingleInstance()
                .WithParameter("attributeType", typeof(ThrottleThisAttribute))
                .Keyed<IAttributeUrls>("ApiThrottler");

            builder.RegisterType<AttributeUrls>()
                .As<IAttributeUrls>()
                .SingleInstance()
                .WithParameter("attributeType", typeof(LogThisAttribute))
                .Keyed<IAttributeUrls>("Logger");

            builder.RegisterType<ResultTrimmer>()
                .As<IResultTrimmer>();
            builder.RegisterType<DatabaseUserDictionary>()
                .As<IUserDictionaryLookup>();
            builder.RegisterType<DatabaseUserDictionary>()
                .As<IExceptionDictionary>();


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
            builder.RegisterType<UserCacheLoaderMiddleware>();
            builder.RegisterType<ThrottlingMiddleware>()
                .WithParameter(new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IAttributeUrls),
                    (pi, ctx) => ctx.ResolveKeyed<IAttributeUrls>("ApiThrottler")));

            builder.RegisterType<LoggingMiddleware>()
                .WithParameter(new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IAttributeUrls),
                    (pi, ctx) => ctx.ResolveKeyed<IAttributeUrls>("Logger")));
        }
    }
}