namespace Morpher.WebApi.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;

    using Moq;

    using Morpher.WebApi.ApiThrottler;
    using Morpher.WebApi.Controllers;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Exceptions;
    using Morpher.WebApi.Services.Interfaces;

    using NUnit.Framework;

    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RussianAnalyzerControllerTests
    {
        [Test]
        public void Declension_ShouldSuccess()
        {
            bool paidUser;
            IApiThrottler apiThrottler = Mock.Of<IApiThrottler>(
                throttler => throttler.Throttle(It.IsAny<HttpRequestMessage>(), out paidUser)
                             == ApiThrottlingResult.Success);

            IMorpherLog log = Mock.Of<IMorpherLog>();

            IRussianAnalyzer analyzer = Mock.Of<IRussianAnalyzer>(
                russianAnalyzer => russianAnalyzer.Declension(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<bool>())
                                   == new RussianDeclensionResult());

            RussianAnalyzerController analyzerController =
                new RussianAnalyzerController(analyzer, apiThrottler, log)
                {
                    Request = new HttpRequestMessage(
                            HttpMethod.Get,
                            $"http://localhost:0/russian/declension?s=any&token={default(Guid)}")
                };
            analyzerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            HttpResponseMessage responseMessage = analyzerController.Declension("any");

            RussianDeclensionResult declensionResult;
            responseMessage.TryGetContentValue(out declensionResult);

            Assert.NotNull(declensionResult);
        }

        [Test]
        public void Declension_Error_Overlimit()
        {
            bool paidUser;
            IApiThrottler apiThrottler = Mock.Of<IApiThrottler>(
                throttler => throttler.Throttle(It.IsAny<HttpRequestMessage>(), out paidUser)
                             == ApiThrottlingResult.Overlimit);

            IMorpherLog log = Mock.Of<IMorpherLog>();

            IRussianAnalyzer analyzer = Mock.Of<IRussianAnalyzer>(
                russianAnalyzer => russianAnalyzer.Declension(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<bool>())
                                   == new RussianDeclensionResult());

            RussianAnalyzerController analyzerController =
                new RussianAnalyzerController(analyzer, apiThrottler, log)
                {
                    Request = new HttpRequestMessage(
                            HttpMethod.Get,
                            $"http://localhost:0/russian/declension?s=any&token={default(Guid)}")
                };
            analyzerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            HttpResponseMessage responseMessage = analyzerController.Declension("any");

            ServiceErrorMessage serviceErrorMessage;
            responseMessage.TryGetContentValue(out serviceErrorMessage);

            Assert.AreEqual(new ServiceErrorMessage(new ExceededDailyLimitException()), serviceErrorMessage);
        }

        [Test]
        public void Declension_Error_WordsNotFound()
        {
            bool paidUser;
            IApiThrottler apiThrottler = Mock.Of<IApiThrottler>(
                throttler => throttler.Throttle(It.IsAny<HttpRequestMessage>(), out paidUser)
                             == ApiThrottlingResult.Overlimit);

            IMorpherLog log = Mock.Of<IMorpherLog>();
            Mock<IRussianAnalyzer> mock = new Mock<IRussianAnalyzer>();
            mock.Setup(
                    russianAnalyzer => russianAnalyzer.Declension(
                        It.IsAny<string>(),
                        It.IsAny<Guid>(),
                        It.IsAny<bool>()))
                .Throws(new WordsNotFoundException());

            IRussianAnalyzer analyzer = mock.Object;

            RussianAnalyzerController analyzerController =
                new RussianAnalyzerController(analyzer, apiThrottler, log)
                {
                    Request = new HttpRequestMessage(
                            HttpMethod.Get,
                            $"http://localhost:0/russian/declension?s=any&token={default(Guid)}")
                };
            analyzerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            HttpResponseMessage responseMessage = analyzerController.Declension("any");

            ServiceErrorMessage serviceErrorMessage;
            responseMessage.TryGetContentValue(out serviceErrorMessage);

            Assert.AreEqual(new ServiceErrorMessage(new ExceededDailyLimitException()), serviceErrorMessage);
        }

        [Test]
        public void Spell_ShouldSuccess()
        {
            bool paidUser;
            IApiThrottler apiThrottler = Mock.Of<IApiThrottler>(
                throttler => throttler.Throttle(It.IsAny<HttpRequestMessage>(), out paidUser)
                             == ApiThrottlingResult.Success);

            IMorpherLog log = Mock.Of<IMorpherLog>();

            IRussianAnalyzer analyzer = Mock.Of<IRussianAnalyzer>(
                russianAnalyzer => russianAnalyzer.Spell(It.IsAny<int>(), It.IsAny<string>())
                                   == new RussianNumberSpelling());

            RussianAnalyzerController analyzerController =
                new RussianAnalyzerController(analyzer, apiThrottler, log)
                {
                    Request = new HttpRequestMessage(
                            HttpMethod.Get,
                            $"http://localhost:0/russian/spell?n={default(int)}&unit=any")
                };
            analyzerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            HttpResponseMessage responseMessage = analyzerController.Spell(default(int), "any");
            RussianNumberSpelling numberSpelling;
            responseMessage.TryGetContentValue(out numberSpelling);

            Assert.NotNull(numberSpelling);
        }

        [Test]
        public void Spell_Error_WordsNotFound()
        {
            bool paidUser;
            IApiThrottler apiThrottler = Mock.Of<IApiThrottler>(
                throttler => throttler.Throttle(It.IsAny<HttpRequestMessage>(), out paidUser)
                             == ApiThrottlingResult.Success);

            IMorpherLog log = Mock.Of<IMorpherLog>();
            Mock<IRussianAnalyzer> mock = new Mock<IRussianAnalyzer>();
            mock.Setup(
                    russianAnalyzer => russianAnalyzer.Spell(It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new WordsNotFoundException());

            IRussianAnalyzer analyzer = mock.Object;

            RussianAnalyzerController analyzerController =
                new RussianAnalyzerController(analyzer, apiThrottler, log)
                    {
                        Request = new HttpRequestMessage(
                            HttpMethod.Get,
                            $"http://localhost:0/russian/spell?n=10&unit=рубль")
                    };
            analyzerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            HttpResponseMessage httpResponseMessage = analyzerController.Spell(10, "any");
            ServiceErrorMessage serviceErrorMessage;
            httpResponseMessage.TryGetContentValue(out serviceErrorMessage);

            Assert.AreEqual(new ServiceErrorMessage(new WordsNotFoundException()), serviceErrorMessage);
        }

        [Test]
        public void Spell_Error_Overlimit()
        {
            bool paidUser;
            IApiThrottler apiThrottler = Mock.Of<IApiThrottler>(
                throttler => throttler.Throttle(It.IsAny<HttpRequestMessage>(), out paidUser)
                             == ApiThrottlingResult.Overlimit);

            IMorpherLog log = Mock.Of<IMorpherLog>();


            RussianAnalyzerController analyzerController =
                new RussianAnalyzerController(null, apiThrottler, log)
                    {
                        Request = new HttpRequestMessage(
                            HttpMethod.Get,
                            $"http://localhost:0/russian/spell?n=10&unit=рубль")
                    };
            analyzerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            HttpResponseMessage httpResponseMessage = analyzerController.Spell(10, "any");
            ServiceErrorMessage serviceErrorMessage;
            httpResponseMessage.TryGetContentValue(out serviceErrorMessage);

            Assert.AreEqual(new ServiceErrorMessage(new ExceededDailyLimitException()), serviceErrorMessage);
        }

        [Test]
        public void Adjectivize_Success()
        {
            bool paidUser;
            IApiThrottler apiThrottler = Mock.Of<IApiThrottler>(
                throttler => throttler.Throttle(It.IsAny<HttpRequestMessage>(), out paidUser)
                             == ApiThrottlingResult.Success);

            IMorpherLog log = Mock.Of<IMorpherLog>();

            IRussianAnalyzer analyzer = Mock.Of<IRussianAnalyzer>(
                russianAnalyzer => russianAnalyzer.Adjectives(It.IsAny<string>())
                                   == new List<string>());

            RussianAnalyzerController analyzerController =
                new RussianAnalyzerController(analyzer, apiThrottler, log)
                    {
                        Request = new HttpRequestMessage(
                            HttpMethod.Get,
                            $"http://localhost:0/russian/adjectivize?s=any")
                    };
            analyzerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            HttpResponseMessage responseMessage = analyzerController.Adjectivize("any");
            List<string> adjectives;
            responseMessage.TryGetContentValue(out adjectives);

            Assert.NotNull(adjectives);
        }

        [Test]
        public void Adjectivize_Error_Overlimit()
        {
            bool paidUser;
            IApiThrottler apiThrottler = Mock.Of<IApiThrottler>(
                throttler => throttler.Throttle(It.IsAny<HttpRequestMessage>(), out paidUser)
                             == ApiThrottlingResult.Overlimit);

            IMorpherLog log = Mock.Of<IMorpherLog>();

            RussianAnalyzerController analyzerController =
                new RussianAnalyzerController(null, apiThrottler, log)
                    {
                        Request = new HttpRequestMessage(
                            HttpMethod.Get,
                            $"http://localhost:0/russian/adjectivize?s=any")
                    };
            analyzerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            HttpResponseMessage responseMessage = analyzerController.Adjectivize("any");
            ServiceErrorMessage serviceErrorMessage;
            responseMessage.TryGetContentValue(out serviceErrorMessage);

            Assert.AreEqual(new ServiceErrorMessage(new ExceededDailyLimitException()), serviceErrorMessage);
        }

        [Test]
        public void Adjectivize_Error_WordsNotFound()
        {
            bool paidUser;
            IApiThrottler apiThrottler = Mock.Of<IApiThrottler>(
                throttler => throttler.Throttle(It.IsAny<HttpRequestMessage>(), out paidUser)
                             == ApiThrottlingResult.Success);

            IMorpherLog log = Mock.Of<IMorpherLog>();

            Mock<IRussianAnalyzer> mock = new Mock<IRussianAnalyzer>();
            mock.Setup(
                    russianAnalyzer => russianAnalyzer.Adjectives(It.IsAny<string>()))
                .Throws(new WordsNotFoundException());

            IRussianAnalyzer analyzer = mock.Object;

            RussianAnalyzerController analyzerController =
                new RussianAnalyzerController(analyzer, apiThrottler, log)
                    {
                        Request = new HttpRequestMessage(
                            HttpMethod.Get,
                            $"http://localhost:0/russian/adjectivize?s=any")
                    };
            analyzerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            HttpResponseMessage responseMessage = analyzerController.Adjectivize("any");
            ServiceErrorMessage serviceErrorMessage;
            responseMessage.TryGetContentValue(out serviceErrorMessage);

            Assert.AreEqual(new ServiceErrorMessage(new WordsNotFoundException()), serviceErrorMessage);
        }
    }
}
