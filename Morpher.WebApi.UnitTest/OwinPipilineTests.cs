namespace Morpher.WebService.V3.UnitTests
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using App_Start;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Microsoft.Owin.Hosting;
    using Microsoft.Owin.Testing;
    using NUnit.Framework;
    using Owin;
    using Services;
    using Services.Interfaces;

    [TestFixture]
    class OwinPipilineTests
    {
        private TestServer PrepareTestServer(ContainerBuilder builder)
        {
            
            HttpConfiguration configuration = new HttpConfiguration();
            TestWebApiResolver apiResolver = new TestWebApiResolver();

            configuration.Services.Replace(typeof(IAssembliesResolver), apiResolver);
            WebApiConfig.Register(configuration);

            builder.RegisterApiControllers(apiResolver.GetAssemblies().First());
            builder.RegisterWebApiModelBinderProvider();

            var container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            
            TestServer testServer = TestServer.Create(appBuilder =>
            {
                appBuilder.UseAutofacMiddleware(container);
                appBuilder.UseWebApi(configuration);
            });

            return testServer;
        }

        [Test]
        public async Task LogwithoutErrors()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RussianWebAnalyzer>().As<IRussianAnalyzer>()
                .WithParameter("client", new MorpherClient().Russian);
            using (var testServer = PrepareTestServer(builder))
            {
                using (var client = new HttpClient(testServer.Handler))
                {
                    var response = await client.GetAsync("http://localhost/russian/declension?s=Пользователь");
                    var result = await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
