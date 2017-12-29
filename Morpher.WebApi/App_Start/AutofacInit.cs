using System.Collections.Specialized;
using Autofac.Configuration;
using Morpher.WebService.V3.Qazaq;
using Morpher.WebService.V3.Russian;
using Morpher.WebService.V3.Russian.Data;
using Morpher.WebService.V3.Ukrainian.Data;

namespace Morpher.WebService.V3
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Web.Http;
    using Autofac;
    using Autofac.Core;
    using Autofac.Integration.WebApi;
    using General.Data;
    using General.Data.Interfaces;
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

            // Exception Filter
            builder.Register(context => new MorpherExceptionFilterAttribute())
                .AsWebApiExceptionFilterFor<ApiController>().SingleInstance();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterWebApiModelBinderProvider();

            bool runAsLocalService = Convert.ToBoolean(
                ((NameValueCollection)ConfigurationManager.GetSection("WebServiceSettings")).Get("RunAsLocalService"));

            if (runAsLocalService)
                RegisterLocal(builder);
            else
                RegisterGlobal(builder);

            Container = builder.Build();
            AutofacWebApiDependencyResolver = new AutofacWebApiDependencyResolver(Container);
            config.DependencyResolver = AutofacWebApiDependencyResolver;
        }

        private static void RegisterGlobal(ContainerBuilder builder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MorpherDatabase"].ConnectionString;
            string externalAnalyzer = ((NameValueCollection)ConfigurationManager.GetSection("WebServiceSettings")).Get("ExternalAnalyzer");

            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "bin",
                externalAnalyzer);
            var analyzer = Assembly.LoadFile(path);

            builder.RegisterAssemblyTypes(analyzer)
                .Where(type => typeof(IMorpher).IsAssignableFrom(type))
                .As<IMorpher>().SingleInstance();

            builder.RegisterAssemblyTypes(analyzer)
                .Where(type => typeof(IAccentizer).IsAssignableFrom(type))
                .As<IAccentizer>().SingleInstance();

            builder.RegisterAssemblyTypes(analyzer)
                .Where(type => typeof(IAdjectivizer).IsAssignableFrom(type))
                .As<IAdjectivizer>().SingleInstance();

            builder.RegisterAssemblyTypes(analyzer)
                .Where(type => typeof(IUkrainianAnalyzer).IsAssignableFrom(type))
                .As<IUkrainianAnalyzer>().SingleInstance();

            builder.RegisterType<MorpherCache>()
                .As<IMorpherCache>()
                .WithParameter("name", "ApiThrottler")
                .SingleInstance();

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


            builder.RegisterType<LogSyncer>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UserCacheSyncer>().AsSelf().InstancePerLifetimeScope();

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

            builder.RegisterType<MorpherCache>()
                .As<ICorrectionCache>()
                .WithParameter("name", "UserCorrection")
                .SingleInstance();


            builder.RegisterType<DatabaseUserDictionary>()
                .As<Russian.IUserDictionaryLookup, Ukrainian.IUserDictionaryLookup>()
                .As<Russian.IExceptionDictionary, Ukrainian.IExceptionDictionary>().SingleInstance();
        }

        private static void RegisterFromAssembly(ContainerBuilder builder, Type type, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .Where(type.IsAssignableFrom)
                .As(type).SingleInstance();
        }

        private static void RegisterLocal(ContainerBuilder builder)
        {
            string binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            string morpherPath = Path.Combine(binPath, "Morpher.dll");
            string accentizerPath = Path.Combine(binPath,"Accentizer2.dll");
            string adjectivizerPath = Path.Combine(binPath, "Adjectivizer.dll");
            string ukrainianPath = Path.Combine(binPath, "Morpher.Ukrainian.dll");
            string qazaqPath = Path.Combine(binPath, "Morpher.Kazakh.dll");

            string externalAnalyzer =
                ((NameValueCollection)ConfigurationManager.GetSection("WebServiceSettings")).Get("ExternalAnalyzer");

            string externalAdapter = Path.Combine(binPath, externalAnalyzer);

            if (File.Exists(externalAdapter))
            {
                var analyzer = Assembly.LoadFile(externalAdapter);

                if (File.Exists(morpherPath))
                {
                    RegisterFromAssembly(builder, typeof(IMorpher), analyzer);
                    builder.RegisterAssemblyTypes(analyzer)
                        .Where(type => typeof(IExceptionDictionary).IsAssignableFrom(type))
                        .As<IExceptionDictionary, IUserDictionaryLookup>().SingleInstance()
                        .WithParameter("userDict", "UserDict.xml");
                }
                else
                {
                    builder.RegisterType<WebAnalyzer>().As<IMorpher>();
                    builder.RegisterType<WebExceptionDictionary>().As<IExceptionDictionary>();
                }

                if (File.Exists(accentizerPath)) RegisterFromAssembly(builder, typeof(IAccentizer), analyzer);
                else builder.RegisterType<WebAnalyzer>().As<IAccentizer>();

                if (File.Exists(adjectivizerPath)) RegisterFromAssembly(builder, typeof(IAdjectivizer), analyzer);
                else builder.RegisterType<WebAnalyzer>().As<IAdjectivizer>();

                if (File.Exists(ukrainianPath))
                {
                    RegisterFromAssembly(builder, typeof(IUkrainianAnalyzer), analyzer);
                    builder.RegisterAssemblyTypes(analyzer)
                        .Where(type => typeof(Ukrainian.IExceptionDictionary).IsAssignableFrom(type))
                        .As<Ukrainian.IExceptionDictionary, Ukrainian.IUserDictionaryLookup>().SingleInstance()
                        .WithParameter("userDict", "UserDictUkr.xml");
                }
                else
                {
                    builder.RegisterType<Ukrainian.WebAnalyzer>().As<IUkrainianAnalyzer>();
                    builder.RegisterType<Ukrainian.WebExceptionDictionary>().As<Ukrainian.IExceptionDictionary>();
                }

                if (File.Exists(qazaqPath)) RegisterFromAssembly(builder, typeof(IQazaqAnalyzer), analyzer);
                else builder.RegisterType<QazaqWebAnalyzer>().As<IQazaqAnalyzer>().SingleInstance();
            }
            else
            {
                builder.RegisterType<WebAnalyzer>().As<IMorpher, IAdjectivizer, IAccentizer>();
                builder.RegisterType<Ukrainian.WebAnalyzer>().As<IUkrainianAnalyzer>();
                builder.RegisterType<Ukrainian.WebExceptionDictionary>().As<Ukrainian.IExceptionDictionary>();
                builder.RegisterType<WebExceptionDictionary>().As<IExceptionDictionary>();
            }

            var conf = (NameValueCollection)ConfigurationManager.GetSection("WebServiceSettings");
            builder.RegisterType<DummyResultTrimmer>().As<IResultTrimmer>();
            Guid token;
            if (Guid.TryParse(conf.Get("MorpherClientToken"), out token))
                builder.RegisterType<MorpherClient>().AsSelf().WithParameter("token", token);
            else
                builder.RegisterType<MorpherClient>().AsSelf();
        }
    }
}