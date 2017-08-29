namespace Morpher.WebService.V3.UnitTests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;

    using Autofac;
    using Autofac.Integration.WebApi;
    using General.Data;
    using General.Data.Services;
    using Microsoft.Owin.Testing;
    using Moq;
    using NUnit.Framework;
    using Owin;
    using DeclensionFlags = General.Data.DeclensionFlags;
    using DeclensionResult = Russian.Data.DeclensionResult;

    [TestFixture]
    class OwinPipilineTests
    {
        private IContainer container;

        private TestServer PrepareTestServer(ContainerBuilder builder)
        {

            HttpConfiguration configuration = new HttpConfiguration();
            TestWebApiResolver apiResolver = new TestWebApiResolver();

            configuration.Services.Replace(typeof(IAssembliesResolver), apiResolver);
            WebApiConfig.Register(configuration);

            builder.RegisterApiControllers(apiResolver.GetAssemblies().First());
            builder.RegisterWebApiFilterProvider(configuration);
            builder.RegisterWebApiModelBinderProvider();
            container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            TestServer testServer = TestServer.Create(appBuilder =>
            {
                appBuilder.UseAutofacMiddleware(container);
                appBuilder.UseWebApi(configuration);
            });

            return testServer;
        }

        private IRussianAnalyzer mockAnalyzer =
            Mock.Of<IRussianAnalyzer>(
                analyzer => analyzer.Declension(It.IsAny<string>(), It.IsAny<DeclensionFlags>()) ==
                            new DeclensionResult());

        class DatabaseLogMock : IDatabaseLog
        {
            public ConcurrentQueue<LogEntity> Logs { get; private set; }

            public void Upload(ConcurrentQueue<LogEntity> logs)
            {
                Logs = logs;
            }
        }

        /// <summary>
        /// Выполняется обычный запрос на склонение
        /// В логи попадает результат без ошибки
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task LogWithoutError()
        {
            // Arrange
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(mockAnalyzer).As<IRussianAnalyzer>();

            // LoggingMiddleware использует два класса для работы с логами в бд.
            DatabaseLogMock databaseLogMock = new DatabaseLogMock();
            builder.RegisterInstance(databaseLogMock).As<IDatabaseLog>().SingleInstance();
            builder.RegisterType<MorpherLog>().As<IMorpherLog>().SingleInstance();
            builder.RegisterType<MorpherCache>()
                .As<IMorpherCache>()
                .WithParameter("name", "ApiThrottler")
                .SingleInstance();

            IAttributeUrls attributeUrls =
                Mock.Of<IAttributeUrls>(urls => urls.Urls == new HashSet<string>()
                {
                    "/russian/declension"
                });

            builder.RegisterInstance(attributeUrls)
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("Logger");

            builder.RegisterType<LoggingMiddleware>();

            using (var testServer = PrepareTestServer(builder))
            {
                // Act
                using (var client = testServer.HttpClient)
                {
                    await client.GetAsync("/russian/declension?s=Пользователь");
                }

                // Assert
                var morpheLog = container.Resolve<IMorpherLog>();
                morpheLog.Sync();
                Assert.AreEqual(1, databaseLogMock.Logs.Count);
                var logEntity = databaseLogMock.Logs.First();
                Assert.AreEqual(0, logEntity.ErrorCode);
            }
        }

        /// <summary>
        /// Вызываем Action без передачи слова
        /// Получаем Исключение
        /// Исключение обрабатывает фильтр
        /// В логах получаем код ошибки
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task LogWithError()
        {
            // Arrange
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(mockAnalyzer).As<IRussianAnalyzer>();

            // LoggingMiddleware использует два класса для работы с логами в бд.
            DatabaseLogMock databaseLogMock = new DatabaseLogMock();
            builder.RegisterInstance(databaseLogMock).As<IDatabaseLog>().SingleInstance();
            builder.RegisterType<MorpherLog>().As<IMorpherLog>().SingleInstance();

            builder.Register(context => new MorpherExceptionFilterAttribute())
                .AsWebApiExceptionFilterFor<ApiController>().SingleInstance();

            builder.RegisterType<MorpherCache>()
                .As<IMorpherCache>()
                .WithParameter("name", "ApiThrottler")
                .SingleInstance();

            IAttributeUrls attributeUrls =
                Mock.Of<IAttributeUrls>(urls => urls.Urls == new HashSet<string>()
                {
                    "/russian/declension"
                });

            builder.RegisterInstance(attributeUrls)
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("Logger");

            builder.RegisterType<LoggingMiddleware>();

            using (var testServer = PrepareTestServer(builder))
            {
                // Act
                using (var client = testServer.HttpClient)
                {
                    await client.GetAsync("/russian/declension?s=");
                }

                // Assert
                var morpheLog = container.Resolve<IMorpherLog>();
                morpheLog.Sync();
                Assert.AreEqual(1, databaseLogMock.Logs.Count);
                var logEntity = databaseLogMock.Logs.First();
                Assert.AreEqual(new RequiredParameterIsNotSpecifiedException("s").Code, logEntity.ErrorCode);
            }
        }

        /// <summary>
        /// Делается 4 запроса без ввода токена.
        /// Первые 3 проходят.
        /// Четвертый Overlimit
        /// </summary>
        [Test]
        public async Task ApiThrottlerTest_ByIp()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Fix user-agent & remote ip
            builder.RegisterType<FixRequestTestDataMiddleware>();
            builder.RegisterType<ThrottlingMiddleware>();
            builder.RegisterType<MorpherCache>()
                .As<IMorpherCache>()
                .WithParameter("name", "ApiThrottler")
                .SingleInstance();
            builder.RegisterInstance(mockAnalyzer).As<IRussianAnalyzer>();
            builder.RegisterType<ApiThrottler>().As<IApiThrottler>();

            IAttributeUrls attributeUrls =
                Mock.Of<IAttributeUrls>(urls => urls.Urls == new HashSet<string>()
                {
                    "/russian/declension"
                });

            builder.RegisterInstance(attributeUrls)
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("ApiThrottler");

            Mock<IMorpherDatabase> morpherDatabaseMock = new Mock<IMorpherDatabase>();
            morpherDatabaseMock.Setup(database => database.IsIpBlocked("0.0.0.0")).Returns(false);
            morpherDatabaseMock.Setup(database => database.GetQueryCountByIp("0.0.0.0")).Returns(0);
            morpherDatabaseMock.Setup(database => database.GetDefaultDailyQueryLimit()).Returns(3);

            builder.RegisterInstance(morpherDatabaseMock.Object).As<IMorpherDatabase>().SingleInstance();

            using (var server = PrepareTestServer(builder))
            {
                using (var client = server.HttpClient)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var result = await client.GetAsync("/russian/declension?s=Тест");
                        Assert.AreEqual(true, result.IsSuccessStatusCode, "StatudCode != OK");
                    }

                    var errorResult = await client.GetAsync("/russian/declension?s=Тест");
                    Assert.AreEqual(HttpStatusCode.BadRequest, errorResult.StatusCode);
                }
            }
        }

        /// <summary>
        /// Делается 4 запроса токен через query string.
        /// Первые 3 проходят.
        /// Четвертый Overlimit
        /// </summary>
        [Test]
        public async Task ApiThrottlerTest_ByToken_QueryString()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Fix user-agent & remote ip
            builder.RegisterType<FixRequestTestDataMiddleware>();
            builder.RegisterType<ThrottlingMiddleware>();
            builder.RegisterType<MorpherCache>()
                .As<IMorpherCache>()
                .WithParameter("name", "ApiThrottler")
                .SingleInstance();
            builder.RegisterInstance(mockAnalyzer).As<IRussianAnalyzer>();
            builder.RegisterType<ApiThrottler>().As<IApiThrottler>();

            IAttributeUrls attributeUrls =
                Mock.Of<IAttributeUrls>(urls => urls.Urls == new HashSet<string>()
                {
                    "/russian/declension"
                });

            builder.RegisterInstance(attributeUrls)
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("ApiThrottler");

            Mock<IMorpherDatabase> morpherDatabaseMock = new Mock<IMorpherDatabase>();
            Guid testToken = Guid.NewGuid();
            morpherDatabaseMock.Setup(database => database.GetQueryCountByToken(testToken)).Returns(0);
            morpherDatabaseMock.Setup(database => database.GetUserLimits(testToken)).Returns(new MorpherCacheObject()
            {
                PaidUser = false,
                UserId = Guid.Empty,
                QueriesLeft = 3,
                Unlimited = false
            });

            builder.RegisterInstance(morpherDatabaseMock.Object).As<IMorpherDatabase>().SingleInstance();

            using (var server = PrepareTestServer(builder))
            {
                using (var client = server.HttpClient)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var result = await client.GetAsync($"/russian/declension?s=Тест&token={testToken}");
                        Assert.AreEqual(true, result.IsSuccessStatusCode, "StatudCode != OK");
                    }

                    var errorResult = await client.GetAsync($"/russian/declension?s=Тест&token={testToken}");
                    Assert.AreEqual(HttpStatusCode.BadRequest, errorResult.StatusCode);
                }
            }
        }


        /// <summary>
        /// Делается 4 запроса, токен через header.
        /// Первые 3 проходят.
        /// Четвертый Overlimit
        /// </summary>
        [Test]
        public async Task ApiThrottlerTest_ByToken_ViaHeader()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Fix user-agent & remote ip
            builder.RegisterType<FixRequestTestDataMiddleware>();
            builder.RegisterType<ThrottlingMiddleware>();
            builder.RegisterType<MorpherCache>()
                .As<IMorpherCache>()
                .WithParameter("name", "ApiThrottler")
                .SingleInstance();
            builder.RegisterInstance(mockAnalyzer).As<IRussianAnalyzer>();
            builder.RegisterType<ApiThrottler>().As<IApiThrottler>();

            IAttributeUrls attributeUrls =
                Mock.Of<IAttributeUrls>(urls => urls.Urls == new HashSet<string>()
                {
                    "/russian/declension"
                });

            builder.RegisterInstance(attributeUrls)
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("ApiThrottler");

            Mock<IMorpherDatabase> morpherDatabaseMock = new Mock<IMorpherDatabase>();
            Guid testToken = Guid.Parse("e4c7d4ca-8037-44fa-a314-2b1717c626d8");
            morpherDatabaseMock.Setup(database => database.GetQueryCountByToken(testToken)).Returns(0);
            morpherDatabaseMock.Setup(database => database.GetUserLimits(testToken)).Returns(new MorpherCacheObject()
            {
                PaidUser = false,
                UserId = Guid.Empty,
                QueriesLeft = 3,
                Unlimited = false
            });

            builder.RegisterInstance(morpherDatabaseMock.Object).As<IMorpherDatabase>().SingleInstance();

            using (var server = PrepareTestServer(builder))
            {
                using (var client = server.HttpClient)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(
                        "Authorization",
                        $"Basic ZTRjN2Q0Y2EtODAzNy00NGZhLWEzMTQtMmIxNzE3YzYyNmQ4Cg==");
                    for (int i = 0; i < 3; i++)
                    {
                        var result = await client.GetAsync($"/russian/declension?s=Тест");
                        Assert.AreEqual(true, result.IsSuccessStatusCode, "StatudCode != OK");
                    }

                    var errorResult = await client.GetAsync($"/russian/declension?s=Тест&token={testToken}");
                    Assert.AreEqual(HttpStatusCode.BadRequest, errorResult.StatusCode);
                }
            }
        }
    }
}
