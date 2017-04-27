namespace Morpher.WebApi.UnitTest
{
    using System;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;

    using Moq;

    using Morpher.WebApi.Analyzers.Interfaces;
    using Morpher.WebApi.ApiThrottler;
    using Morpher.WebApi.Controllers;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Services.Interfaces;

    using NUnit.Framework;

    [TestFixture]
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

            Assert.NotNull(declensionResult, "RussianDeclensionResult == null");
        }
    }
}
