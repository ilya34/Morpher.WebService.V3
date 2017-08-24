namespace Morpher.WebService.V3.App_Start
{
    using System.Configuration;
    using System.Reflection;
    using System.Web.Http;
    using Autofac;
    using Autofac.Core;
    using Autofac.Features.AttributeFilters;
    using Autofac.Integration.WebApi;
    using Filters;
    using Helpers;
    using Middlewares;
    using Services;
    using Services.Interfaces;

    public static class AutofacInit
    {
        public static IContainer Container { get; private set; }

        public static void Init()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterWebApiModelBinderProvider();

            RegisterServices(builder);

            Container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(Container);

        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MorpherDatabase"].ConnectionString;

            builder.RegisterType<AttributeUrls>()
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("ApiThrottler")
                .SingleInstance()
                .WithParameter("attributeType", typeof(ThrottleThisAttribute));

            builder.RegisterType<RussianWebAnalyzer>().As<IRussianAnalyzer>().WithParameter("client", new MorpherClient());
            builder.RegisterType<ApiThrottler>().As<IApiThrottler>();
            builder.RegisterType<MorpherCache>().As<IMorpherCache>().WithParameter("name", "ApiThrottler");
            builder.RegisterType<MorpherDatabase>().As<IMorpherDatabase>()
                .WithParameter("connectionString", connectionString);


            // Middlewares
            builder.RegisterType<ThrottlingMiddleware>();

            // Filters
            builder.Register(context => new MorpherExceptionFilterAttribute())
                .AsWebApiExceptionFilterFor<ApiController>().SingleInstance();
        }
    }
}