using System.Collections.Specialized;

using Ru = Morpher.WebService.V3.Russian.Data;
using Uk = Morpher.WebService.V3.Ukrainian.Data;

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
    using General;

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
            builder.RegisterType<ExceptionHandlingAndLoggingMiddleware>();
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
                .Where(type => typeof(Ru.IMorpher).IsAssignableFrom(type))
                .As<Ru.IMorpher>().SingleInstance();

            builder.RegisterAssemblyTypes(analyzer)
                .Where(type => typeof(Ru.IAccentizer).IsAssignableFrom(type))
                .As<Ru.IAccentizer>().SingleInstance();

            builder.RegisterAssemblyTypes(analyzer)
                .Where(type => typeof(Ru.IAdjectivizer).IsAssignableFrom(type))
                .As<Ru.IAdjectivizer>().SingleInstance();

            builder.RegisterAssemblyTypes(analyzer)
                .Where(type => typeof(Uk.IUkrainianAnalyzer).IsAssignableFrom(type))
                .As<Uk.IUkrainianAnalyzer>().SingleInstance();

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

            builder.RegisterType<ExceptionHandlingAndLoggingMiddleware>()
                .WithParameter(new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IAttributeUrls),
                    (pi, ctx) => ctx.ResolveKeyed<IAttributeUrls>("Logger")));

            builder.RegisterType<MorpherCache>()
                .As<ICorrectionCache>()
                .WithParameter("name", "UserCorrection")
                .SingleInstance();


            builder.RegisterType<DatabaseUserDictionary>()
                .As<Uk.IUserDictionaryLookup, Uk.IUserDictionaryLookup>()
                .As<Uk.IExceptionDictionary, Uk.IExceptionDictionary>().SingleInstance();
        }

        private static void RegisterFromAssembly(ContainerBuilder builder, Type type, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .Where(type.IsAssignableFrom)
                .As(type).SingleInstance();
        }

        private static void RegisterLocal(ContainerBuilder builder)
        {
            builder.RegisterType<General.DummyServices.ApiThrottler>().As<IApiThrottler>();
            builder.RegisterType<General.DummyServices.MorpherCache>().As<IMorpherCache>();
            builder.RegisterType<General.DummyServices.MorpherDatabase>().As<IMorpherDatabase>();
            builder.RegisterType<General.DummyServices.ResultTrimmer>().As<IResultTrimmer>();
            builder.RegisterType<General.DummyServices.MorpherLog>().As<IMorpherLog>();

            string binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            string morpherPath = Path.Combine(binPath, "Morpher.dll");
            string accentizerPath = Path.Combine(binPath,"Accentizer2.dll");
            string adjectivizerPath = Path.Combine(binPath, "Adjectivizer.dll");
            string ukrainianPath = Path.Combine(binPath, "Morpher.Ukrainian.dll");

            string externalAnalyzer =
                ((NameValueCollection)ConfigurationManager.GetSection("WebServiceSettings")).Get("ExternalAnalyzer");

            string externalAdapter = Path.Combine(binPath, externalAnalyzer);

            if (File.Exists(externalAdapter))
            {
                var analyzer = Assembly.LoadFile(externalAdapter);

                if (File.Exists(morpherPath))
                {
                    RegisterFromAssembly(builder, typeof(Ru.IMorpher), analyzer);
                    builder.RegisterAssemblyTypes(analyzer)
                        .Where(type => typeof(Ru.IExceptionDictionary).IsAssignableFrom(type))
                        .As<Ru.IExceptionDictionary, Ru.IUserDictionaryLookup>().SingleInstance()
                        .WithParameter("userDict", "UserDict.xml");
                }
                else
                {
                    builder.RegisterType<Ru.WebAnalyzer>().As<Ru.IMorpher>();
                    builder.RegisterType<Ru.WebExceptionDictionary>().As<Ru.IExceptionDictionary>();
                }

                if (File.Exists(accentizerPath)) RegisterFromAssembly(builder, typeof(Ru.IAccentizer), analyzer);
                else builder.RegisterType<Ru.WebAnalyzer>().As<Ru.IAccentizer>();

                if (File.Exists(adjectivizerPath)) RegisterFromAssembly(builder, typeof(Ru.IAdjectivizer), analyzer);
                else builder.RegisterType<Ru.WebAnalyzer>().As<Ru.IAdjectivizer>();

                if (File.Exists(ukrainianPath))
                {
                    RegisterFromAssembly(builder, typeof(Uk.IUkrainianAnalyzer), analyzer);
                    builder.RegisterAssemblyTypes(analyzer)
                        .Where(type => typeof(Uk.IExceptionDictionary).IsAssignableFrom(type))
                        .As<Uk.IExceptionDictionary, Uk.IUserDictionaryLookup>().SingleInstance()
                        .WithParameter("userDict", "UserDictUkr.xml");
                }
                else
                {
                    builder.RegisterType<Uk.WebAnalyzer>().As<Uk.IUkrainianAnalyzer>();
                    builder.RegisterType<Uk.WebExceptionDictionary>().As<Uk.IExceptionDictionary>();
                }
            }
            else
            {
                builder.RegisterType<Ru.WebAnalyzer>().As<Ru.IMorpher, Ru.IAdjectivizer, Ru.IAccentizer>();
                builder.RegisterType<Uk.WebAnalyzer>().As<Uk.IUkrainianAnalyzer>();
                builder.RegisterType<Uk.WebExceptionDictionary>().As<Uk.IExceptionDictionary>();
                builder.RegisterType<Uk.WebExceptionDictionary>().As<Uk.IExceptionDictionary>();
            }

            var conf = (NameValueCollection)ConfigurationManager.GetSection("WebServiceSettings");
            Guid token;
            if (Guid.TryParse(conf.Get("MorpherClientToken"), out token))
                builder.RegisterType<MorpherClient>().AsSelf().WithParameter("token", token);
            else
                builder.RegisterType<MorpherClient>().AsSelf();
        }
    }
}