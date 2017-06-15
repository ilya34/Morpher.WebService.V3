[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Morpher.WebApi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Morpher.WebApi.App_Start.NinjectWebCommon), "Stop")]

namespace Morpher.WebApi.App_Start
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Web;
    using System.Web.Http;
    using System.Linq;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Morpher.WebService.V3.Services;
    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Interfaces;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.WebApi;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            bool isLocal = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]);

            // Если проект запущен как Local, то использутся заглушки для тарифов, и логов.
            // UserCorrection должен смотреть в файл, а не SQL бд
            kernel.Bind<IUserCorrection>().To<UserCorrectionService>();

            if (isLocal)
            {
                kernel.Bind<IUserCorrectionSource>().To<UserCorrectionSourceFile>();
                kernel.Bind<IApiThrottler>().ToConstant(new ApiThrottlerLocal());
                kernel.Bind<IMorpherLog>().ToConstant(new MorpherLogLocal());
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MorpherDatabase"].ConnectionString;
                kernel.Bind<IDatabaseLog>()
                    .To<DatabaseLog>()
                    .WithConstructorArgument("connectionString", connectionString);
                kernel.Bind<IMorpherDatabase>()
                    .To<MorpherDatabase>()
                    .WithConstructorArgument("connectionString", connectionString);
                kernel.Bind<IMorpherCache>().ToConstant(new MorpherCache("ApiTrottler")).Named("ApiThrottler");
                kernel.Bind<IMorpherCache>().ToConstant(new MorpherCache("UserCorrection")).Named("UserCorrection");
                kernel.Bind<IUserCorrectionSource>().To<UserCorrectionSourceDatabase>();
                kernel.Bind<IApiThrottler>().To<ApiThrottler>();
                kernel.Bind<IMorpherLog>().To<MorpherLog>().InSingletonScope();
            }

            bool customInflector = Convert.ToBoolean(ConfigurationManager.AppSettings["CustomInflector"]);

            if (!customInflector)
            {
                kernel.Bind<IRussianAnalyzer>().To<RussianWebAnalyzer>().InSingletonScope()
                    .WithConstructorArgument("isLocalService", isLocal);
                kernel.Bind<IUkrainianAnalyzer>().To<UkrainianWebAnalyzer>().InSingletonScope()
                    .WithConstructorArgument("isLocalService", isLocal);
            }
            else
            {
                string path = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "bin",
                    ConfigurationManager.AppSettings["Inflector"]);
                Assembly assembly = Assembly.LoadFile(path);
                Type russianInflector = assembly.GetTypes()
                    .Single(type => typeof(IRussianAnalyzer).IsAssignableFrom(type));
                Type ukrainianInflector = assembly.GetTypes()
                    .Single(type => typeof(IUkrainianAnalyzer).IsAssignableFrom(type));

                kernel.Bind<IRussianAnalyzer>().To(russianInflector).InSingletonScope()
                    .WithConstructorArgument("isLocalService", isLocal);
                kernel.Bind<IUkrainianAnalyzer>().To(ukrainianInflector).InSingletonScope()
                    .WithConstructorArgument("isLocalService", isLocal);
            }
        }
    }
}
