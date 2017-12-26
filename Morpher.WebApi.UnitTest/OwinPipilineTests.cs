using Morpher.WebService.V3.Russian.Data;

namespace Morpher.WebService.V3.UnitTests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;

    using Autofac;
    using Autofac.Integration.WebApi;
    using General;
    using Microsoft.Owin.Testing;
    using Moq;
    using NUnit.Framework;
    using Owin;
    using DeclensionFlags = General.DeclensionFlags;
    using DeclensionResult = Russian.Data.DeclensionResult;

    [TestFixture]
    class OwinPipilineTests
    {
        private IContainer _container;

        private TestServer PrepareTestServer(ContainerBuilder builder)
        {

            HttpConfiguration configuration = new HttpConfiguration()
            {
                IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always
            };
            TestWebApiResolver apiResolver = new TestWebApiResolver();

            configuration.Services.Replace(typeof(IAssembliesResolver), apiResolver);
            WebApiConfig.Register(configuration);


            builder.RegisterInstance(Mock.Of<IAccentizer>());
            builder.RegisterInstance(Mock.Of<IAdjectivizer>());

            builder.RegisterInstance(
                Mock.Of<IUserDictionaryLookup>(lookup => lookup.Lookup(It.IsAny<string>()) == null))
                .As<IUserDictionaryLookup>();
            builder.RegisterInstance(
                    Mock.Of<IResultTrimmer>(trimmer => true))
                .As<IResultTrimmer>();
            builder.RegisterInstance(
                    Mock.Of<IExceptionDictionary>())
                .As<IExceptionDictionary>();


            builder.RegisterApiControllers(apiResolver.GetAssemblies().First());
            builder.RegisterWebApiFilterProvider(configuration);
            builder.RegisterWebApiModelBinderProvider();
            _container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(_container);

            TestServer testServer = TestServer.Create(appBuilder =>
            {
                appBuilder.UseAutofacMiddleware(_container);
                appBuilder.UseWebApi(configuration);
            });

            return testServer;
        }

        private readonly IMorpher mockAnalyzer =
            Mock.Of<IMorpher>(
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
            builder.RegisterInstance(mockAnalyzer).As<IMorpher>();

            // LoggingMiddleware использует два класса для работы с логами в бд.
            DatabaseLogMock databaseLogMock = new DatabaseLogMock();
            builder.RegisterInstance(databaseLogMock).As<IDatabaseLog>().SingleInstance();
            builder.RegisterType<MorpherLog>().As<IMorpherLog>().SingleInstance();
            builder.RegisterType<MorpherCache>()
                .As<IMorpherCache>()
                .WithParameter("name", "ApiThrottler")
                .SingleInstance();

            IAttributeUrls attributeUrls =
                Mock.Of<IAttributeUrls>(urls => urls.Urls == new Dictionary<string, ThrottleThisAttribute>()
                {
                    { "get:/russian/declension", new ThrottleThisAttribute(1, TarificationMode.PerRequest) }
                });

            builder.RegisterInstance(attributeUrls)
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("Logger");

            builder.RegisterType<ExceptionHandlingAndLoggingMiddleware>();

            using (var testServer = PrepareTestServer(builder))
            {
                // Act
                using (var client = testServer.HttpClient)
                {
                    var result = await client.GetAsync("/russian/declension?s=Пользователь");
                    if (result.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new Exception(await result.Content.ReadAsStringAsync());
                    }
                }

                // Assert
                var morpheLog = _container.Resolve<IMorpherLog>();
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
            builder.RegisterInstance(mockAnalyzer).As<IMorpher>();

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
                Mock.Of<IAttributeUrls>(urls => urls.Urls == new Dictionary<string, ThrottleThisAttribute>()
                {
                    { "get:/russian/declension", new ThrottleThisAttribute(1, TarificationMode.PerRequest) }
                });

            builder.RegisterInstance(attributeUrls)
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("Logger");

            builder.RegisterType<ExceptionHandlingAndLoggingMiddleware>();

            using (var testServer = PrepareTestServer(builder))
            {
                // Act
                using (var client = testServer.HttpClient)
                {
                    var result = await client.GetAsync("/russian/declension?s=");
                    if (result.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new Exception(await result.Content.ReadAsStringAsync());
                    }
                }

                // Assert
                var morpheLog = _container.Resolve<IMorpherLog>();
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
            builder.RegisterInstance(Mock.Of<IMorpherLog>()).As<IMorpherLog>();
            builder.RegisterType<ExceptionHandlingAndLoggingMiddleware>();
            builder.RegisterType<ThrottlingMiddleware>();
            builder.RegisterType<MorpherCache>()
                .As<IMorpherCache>()
                .WithParameter("name", "ApiThrottler")
                .SingleInstance();
            builder.RegisterInstance(mockAnalyzer).As<IMorpher>();
            builder.RegisterType<ApiThrottler>().As<IApiThrottler>();

            IAttributeUrls attributeUrls =
                Mock.Of<IAttributeUrls>(urls => urls.Urls == new Dictionary<string, ThrottleThisAttribute>()
                {
                    { "get:/russian/declension", new ThrottleThisAttribute(1, TarificationMode.PerRequest) }
                });

            builder.RegisterInstance(attributeUrls)
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("ApiThrottler");

            Mock<IMorpherDatabase> morpherDatabaseMock = new Mock<IMorpherDatabase>();
            morpherDatabaseMock.Setup(database => database.IsIpBlocked("0.0.0.0")).Returns(false);
            morpherDatabaseMock.Setup(database => database.GetDefaultDailyQueryLimit()).Returns(3);

            builder.RegisterInstance(morpherDatabaseMock.Object).As<IMorpherDatabase>().SingleInstance();

            using (var server = PrepareTestServer(builder))
            {
                using (var client = server.HttpClient)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var result = await client.GetAsync("/russian/declension?s=Тест");
                        if (result.StatusCode == HttpStatusCode.InternalServerError)
                        {
                            throw new Exception(await result.Content.ReadAsStringAsync());
                        }

                        Assert.AreEqual(true, result.IsSuccessStatusCode, "StatudCode != OK");
                    }

                    var errorResult = await client.GetAsync("/russian/declension?s=Тест");
                    Assert.AreEqual(HttpStatusCode.PaymentRequired, errorResult.StatusCode);
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
            builder.RegisterInstance(Mock.Of<IMorpherLog>()).As<IMorpherLog>();
            builder.RegisterType<ExceptionHandlingAndLoggingMiddleware>();
            builder.RegisterType<ThrottlingMiddleware>();
            builder.RegisterType<MorpherCache>()
                .As<IMorpherCache>()
                .WithParameter("name", "ApiThrottler")
                .SingleInstance();
            builder.RegisterInstance(mockAnalyzer).As<IMorpher>();
            builder.RegisterType<ApiThrottler>().As<IApiThrottler>();

            IAttributeUrls attributeUrls =
                Mock.Of<IAttributeUrls>(urls => urls.Urls == new Dictionary<string, ThrottleThisAttribute>()
                {
                    { "get:/russian/declension", new ThrottleThisAttribute(1, TarificationMode.PerRequest) }
                });

            builder.RegisterInstance(attributeUrls)
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("ApiThrottler");

            Mock<IMorpherDatabase> morpherDatabaseMock = new Mock<IMorpherDatabase>();
            Guid testToken = Guid.NewGuid();
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

                        if (result.StatusCode == HttpStatusCode.InternalServerError)
                        {
                            throw new Exception(await result.Content.ReadAsStringAsync());
                        }

                        Assert.AreEqual(true, result.IsSuccessStatusCode, "StatudCode != OK");
                    }

                    var errorResult = await client.GetAsync($"/russian/declension?s=Тест&token={testToken}");
                    Assert.AreEqual(HttpStatusCode.PaymentRequired, errorResult.StatusCode);
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
            builder.RegisterType<ExceptionHandlingAndLoggingMiddleware>();
            builder.RegisterInstance(Mock.Of<IMorpherLog>()).As<IMorpherLog>();
            builder.RegisterType<ThrottlingMiddleware>();
            builder.RegisterType<MorpherCache>()
                .As<IMorpherCache>()
                .WithParameter("name", "ApiThrottler")
                .SingleInstance();
            builder.RegisterInstance(mockAnalyzer).As<IMorpher>();
            builder.RegisterType<ApiThrottler>().As<IApiThrottler>();

            IAttributeUrls attributeUrls =
                Mock.Of<IAttributeUrls>(urls => urls.Urls == new Dictionary<string, ThrottleThisAttribute>()
                {
                    { "get:/russian/declension", new ThrottleThisAttribute(1, TarificationMode.PerRequest) }
                });

            builder.RegisterInstance(attributeUrls)
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("ApiThrottler");

            Mock<IMorpherDatabase> morpherDatabaseMock = new Mock<IMorpherDatabase>();
            Guid testToken = Guid.Parse("e4c7d4ca-8037-44fa-a314-2b1717c626d8");
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

                        if (result.StatusCode == HttpStatusCode.InternalServerError)
                        {
                            throw new Exception(await result.Content.ReadAsStringAsync());
                        }

                        Assert.AreEqual(true, result.IsSuccessStatusCode, "StatudCode != OK");
                    }

                    var errorResult = await client.GetAsync($"/russian/declension?s=Тест&token={testToken}");
                    Assert.AreEqual(HttpStatusCode.PaymentRequired, errorResult.StatusCode);
                }
            }
        }

        [Test]
        public async Task ApiThrottlerTest_BySymbol()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Fix user-agent & remote ip
            builder.RegisterType<FixRequestTestDataMiddleware>();
            builder.RegisterType<ThrottlingMiddleware>();
            MorpherCache cache = new MorpherCache("ApiThrottler");
            builder.RegisterInstance(cache).As<IMorpherCache>();
            builder.RegisterInstance(mockAnalyzer).As<IMorpher>();
            builder.RegisterType<ApiThrottler>().As<IApiThrottler>();

            IAttributeUrls attributeUrls =
                Mock.Of<IAttributeUrls>(urls => urls.Urls == new Dictionary<string, ThrottleThisAttribute>()
                {
                    { "post:/russian/addstressmarks", new ThrottleThisAttribute(10, TarificationMode.PerSymbol) }
                });

            builder.RegisterInstance(attributeUrls)
                .As<IAttributeUrls>()
                .Keyed<IAttributeUrls>("ApiThrottler");

            Mock<IMorpherDatabase> morpherDatabaseMock = new Mock<IMorpherDatabase>();
            morpherDatabaseMock.Setup(database => database.IsIpBlocked("0.0.0.0")).Returns(false);
            morpherDatabaseMock.Setup(database => database.GetDefaultDailyQueryLimit()).Returns(3);

            builder.RegisterInstance(morpherDatabaseMock.Object).As<IMorpherDatabase>().SingleInstance();

            using (var server = PrepareTestServer(builder))
            {
                using (var client = server.HttpClient)
                {
                    var result = await client.PostAsync("/russian/addstressmarks",
                        new StringContent("здесь 17 символов", Encoding.UTF8, "text/plain"));
                    if (result.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new Exception(await result.Content.ReadAsStringAsync());
                    }

                    Assert.AreEqual(true, result.IsSuccessStatusCode, "StatudCode != OK");
                    var cacheEntry = (MorpherCacheObject)cache.Get("0.0.0.0");
                    Assert.AreEqual(1, cacheEntry.QueriesLeft);
                }
            }
        }
    }
}
