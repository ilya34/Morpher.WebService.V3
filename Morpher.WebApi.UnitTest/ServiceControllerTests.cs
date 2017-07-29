// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.UnitTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;

    using Moq;

    using Morpher.WebService.V3.Controllers;
    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Models.Exceptions;

    using NUnit.Framework;

    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ServiceControllerTests
    {
        [Test]
        public void QueriesLeftToday_TokenNotFound()
        {
            var apiThrottlerMock = new Mock<IApiThrottler>();
            apiThrottlerMock.Setup(throttler => throttler.GetQueryLimit(It.IsAny<Guid>())).Returns((MorpherCacheObject)null);

            Guid guid = Guid.NewGuid();
            HttpRequestMessage requestMessage =
                RequestCreater.CreateRequest($"http://localhost:0/foo?token={guid}", HttpMethod.Get, "::1");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var serviceController = new ServiceController(apiThrottlerMock.Object, null) { Request = requestMessage };
            HttpResponseMessage responseMessage = serviceController.QueriesLeftToday();

            ServiceErrorMessage errorMessage;
            responseMessage.TryGetContentValue(out errorMessage);

            Assert.AreEqual(new ServiceErrorMessage(new TokenNotFoundException()), errorMessage);
        }

        [Test]
        public void QueriesLeftToday_TokenFormatException()
        {
            var apiThrottlerMock = new Mock<IApiThrottler>();
            apiThrottlerMock.Setup(throttler => throttler.GetQueryLimit(It.IsAny<Guid>())).Returns((MorpherCacheObject)null);
            HttpRequestMessage requestMessage =
                RequestCreater.CreateRequest("http://localhost:0/foo?token=invalid_token", HttpMethod.Get, "::1");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var serviceController = new ServiceController(apiThrottlerMock.Object, null) { Request = requestMessage };
            HttpResponseMessage responseMessage = serviceController.QueriesLeftToday();

            ServiceErrorMessage errorMessage;
            responseMessage.TryGetContentValue(out errorMessage);

            Assert.AreEqual(new ServiceErrorMessage(new InvalidTokenFormat()), errorMessage);
        }

        [Test]
        public void QueriesLeftToday_Token_NegativeValue()
        {
            var apiThrottlerMock = new Mock<IApiThrottler>();
            apiThrottlerMock.Setup(throttler => throttler.GetQueryLimit(It.IsAny<Guid>())).Returns(new MorpherCacheObject() { QueriesLeft = -1 });

            Guid guid = Guid.NewGuid();
            HttpRequestMessage requestMessage =
                RequestCreater.CreateRequest($"http://localhost:0/foo?token={guid}", HttpMethod.Get, "::1");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var serviceController = new ServiceController(apiThrottlerMock.Object, null) { Request = requestMessage };
            HttpResponseMessage responseMessage = serviceController.QueriesLeftToday();

            int value;
            responseMessage.TryGetContentValue(out value);

            Assert.AreEqual(0, value);
        }

        [Test]
        public void QueriesLeftToday_Token_PositiveValue()
        {
            var apiThrottlerMock = new Mock<IApiThrottler>();
            apiThrottlerMock.Setup(throttler => throttler.GetQueryLimit(It.IsAny<Guid>())).Returns(new MorpherCacheObject() { QueriesLeft = 5 });

            Guid guid = Guid.NewGuid();
            HttpRequestMessage requestMessage =
                RequestCreater.CreateRequest($"http://localhost:0/foo?token={guid}", HttpMethod.Get, "::1");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var serviceController = new ServiceController(apiThrottlerMock.Object, null) { Request = requestMessage };
            HttpResponseMessage responseMessage = serviceController.QueriesLeftToday();

            int value;
            responseMessage.TryGetContentValue(out value);

            Assert.AreEqual(5, value);
        }

        [Test]
        public void QueriesLeftToday_IpBlocked()
        {
            var apiThrottlerMock = new Mock<IApiThrottler>();
            apiThrottlerMock.Setup(throttler => throttler.GetQueryLimit(It.IsAny<string>())).Returns((MorpherCacheObject)null);

            HttpRequestMessage requestMessage =
                RequestCreater.CreateRequest("http://localhost:0/foo", HttpMethod.Get, "::1");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var serviceController = new ServiceController(apiThrottlerMock.Object, null) { Request = requestMessage };
            HttpResponseMessage responseMessage = serviceController.QueriesLeftToday();

            ServiceErrorMessage errorMessage;
            responseMessage.TryGetContentValue(out errorMessage);

            Assert.AreEqual(new ServiceErrorMessage(new IpBlockedException()), errorMessage);
        }
    }
}
