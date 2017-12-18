using Autofac.Configuration;

namespace Morpher.WebService.V3
{
    using System;
    using System.Configuration;
    using System.Reflection;
    using System.Web.Http;
    using Autofac;
    using Autofac.Core;
    using Autofac.Integration.WebApi;
    using General.Data;
    using General.Data.Middlewares;
    using General.Data.Services;

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
            builder.RegisterModule(new ConfigurationSettingsReader("autofac", "InjectionConfigs/WS3.config"));
            Container = builder.Build();
            AutofacWebApiDependencyResolver = new AutofacWebApiDependencyResolver(Container);
            config.DependencyResolver = AutofacWebApiDependencyResolver;
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            bool runAsLocalService = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("RunAsLocalService"));

            if (!runAsLocalService)
            {
                RegisterGlobalOnlyServices(builder);
            }

            RegisterSharedServices(builder);
        }
        
        private static void RegisterSharedServices(ContainerBuilder builder)
        {
            // Filters
            builder.Register(context => new MorpherExceptionFilterAttribute())
                .AsWebApiExceptionFilterFor<ApiController>().SingleInstance();
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


            // Используется в LoggingMiddleware
            //builder.RegisterType<MorpherLog>().As<IMorpherLog>().SingleInstance();
            builder.RegisterType<DatabaseLog>().As<IDatabaseLog>()
                .WithParameter("connectionString", connectionString);

            // Исплользуется в ThrottlingMIddleware
            //builder.RegisterType<ApiThrottler>().As<IApiThrottler>();
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