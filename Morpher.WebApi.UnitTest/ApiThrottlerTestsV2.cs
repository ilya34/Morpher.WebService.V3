namespace Morpher.WebApi.UnitTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web;

    using Moq;

    using Morpher.WebApi.ApiThrottler;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Services;
    using Morpher.WebApi.Services.Interfaces;

    using NUnit.Framework;

    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ApiThrottlerTestsV2
    {
        [Test]
        public void Throttle_ByIp()
        {
            int queryLimit = 10;
            int overlimitAttempts = 10;
            int recordsInDatabase = 5;

            // Setup database
            Mock<IMorpherDatabase> databaseMock = new Mock<IMorpherDatabase>();
            databaseMock.Setup(database => database.IsIpBlocked(It.IsAny<string>())).Returns(false);
            databaseMock.Setup(database => database.GetDefaultDailyQueryLimit()).Returns(queryLimit);
            databaseMock.Setup(database => database.GetQueryCountByIp(It.IsAny<string>())).Returns(recordsInDatabase)
                .Verifiable();

            // Setup ApiThrottler
            IApiThrottler apiThrottler = new ApiThrottler(databaseMock.Object, new MorpherCache("Throttle_ByIp"));

            // Setup HttpRequest
            HttpRequestMessage requestMessage = this.CreateRequest("http://localhost:0/foo", HttpMethod.Get);

            // Act
            bool paidUser;
            int success = 0;
            for (int i = 0; i < queryLimit - recordsInDatabase; i++)
            {
                ApiThrottlingResult apiThrottlingResult = apiThrottler.Throttle(requestMessage, out paidUser);

                if (apiThrottlingResult == ApiThrottlingResult.Success)
                {
                    success++;
                }
            }

            Assert.AreEqual(queryLimit - recordsInDatabase, success);
            Assert.AreEqual(0, apiThrottler.GetQueryLimit("::1").DailyLimit);

            int overlimit = 0;
            for (int i = 0; i < overlimitAttempts; i++)
            {
                ApiThrottlingResult apiThrottlingResult = apiThrottler.Throttle(requestMessage, out paidUser);

                if (apiThrottlingResult == ApiThrottlingResult.Overlimit)
                {
                    overlimit++;
                }
            }

            Assert.AreEqual(0, apiThrottler.GetQueryLimit("::1").DailyLimit);
            Assert.AreEqual(overlimitAttempts, overlimit);

            databaseMock.Verify(database => database.GetQueryCountByIp(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public void Throttle_ByIp_Overlimit_InDatabase()
        {
            int queryLimit = 10;
            int recordsInDatabase = 15;

            // Setup database
            Mock<IMorpherDatabase> databaseMock = new Mock<IMorpherDatabase>();
            databaseMock.Setup(database => database.IsIpBlocked(It.IsAny<string>())).Returns(false);
            databaseMock.Setup(database => database.GetDefaultDailyQueryLimit()).Returns(queryLimit);
            databaseMock.Setup(database => database.GetQueryCountByIp(It.IsAny<string>())).Returns(recordsInDatabase)
                .Verifiable();

            // Setup ApiThrottler
            IApiThrottler apiThrottler = new ApiThrottler(databaseMock.Object, new MorpherCache("Throttle_ByIp"));

            // Setup HttpRequest
            HttpRequestMessage requestMessage = this.CreateRequest("http://localhost:0/foo", HttpMethod.Get);

            // Act
            bool paidUser;
            ApiThrottlingResult apiThrottlingResult = apiThrottler.Throttle(requestMessage, out paidUser);

            Assert.AreEqual(ApiThrottlingResult.Overlimit, apiThrottlingResult);
            Assert.AreEqual(0, apiThrottler.GetQueryLimit("::1").DailyLimit);
        }

        [Test]
        public void Throttle_IpBlock()
        {
            // Setup database
            Mock<IMorpherDatabase> databaseMock = new Mock<IMorpherDatabase>();
            databaseMock.Setup(database => database.IsIpBlocked(It.IsAny<string>())).Returns(true);

            // Setup ApiThrottler
            IApiThrottler apiThrottler = new ApiThrottler(databaseMock.Object, new MorpherCache("Throttle_IpBlock"));

            // Setup HttpRequest
            HttpRequestMessage requestMessage = this.CreateRequest("http://localhost:0/foo", HttpMethod.Get);

            bool paidUser;
            ApiThrottlingResult apiThrottlingResult = apiThrottler.Throttle(requestMessage, out paidUser);

            Assert.AreEqual(ApiThrottlingResult.IpBlocked, apiThrottlingResult);
        }

        [Test]
        public void Throttle_ByToken_FromQueryString_PaidUser()
        {
            int queryLimit = 10;
            int overlimitAttempts = 10;
            int recordsInDatabase = 5;

            CacheObject cacheObject = new CacheObject() { DailyLimit = queryLimit, PaidUser = true, Unlimited = false };

            Guid guid = Guid.NewGuid();
            HttpRequestMessage requestMessage =
                this.CreateRequest($"http://localhost:0/foo?token={guid}", HttpMethod.Get);

            this.Throttle_ByToken(cacheObject, requestMessage, queryLimit, overlimitAttempts, recordsInDatabase);
        }

        [Test]
        public void Throttle_ByToken_FromQueryString_PaidUser_Overlimit_InDatabase()
        {
            int queryLimit = 10;
            int recordsInDatabase = 15;

            CacheObject cacheObject = new CacheObject() { DailyLimit = queryLimit, PaidUser = true, Unlimited = false };

            Guid guid = Guid.NewGuid();

            // Setup HttpRequest
            HttpRequestMessage requestMessage =
                this.CreateRequest($"http://localhost:0/foo?token={guid}", HttpMethod.Get);

            this.Throttle_ByToken_Overlimit_InDatabase(cacheObject, requestMessage, queryLimit, recordsInDatabase);
        }

        [Test]
        public void Throttle_ByToken_FromHeader_PaidUser()
        {
            int queryLimit = 10;
            int overlimitAttempts = 10;
            int recordsInDatabase = 5;

            CacheObject cacheObject = new CacheObject() { DailyLimit = queryLimit, PaidUser = true, Unlimited = false };

            HttpRequestMessage requestMessage =
                this.CreateRequest($"http://localhost:0/foo", HttpMethod.Get);
            requestMessage.Headers.TryAddWithoutValidation("Authorization", "Basic NzE5ODRiMWItYTlhYy00NTc0LWIzZjYtNDhmNzkzMTIxMTEwCg==");

            this.Throttle_ByToken(cacheObject, requestMessage, queryLimit, overlimitAttempts, recordsInDatabase);
        }

        [Test]
        public void Throttle_ByToken_FromHeader_PaidUser_Overlimit_InDatabase()
        {
            int queryLimit = 10;
            int recordsInDatabase = 15;

            CacheObject cacheObject = new CacheObject() { DailyLimit = queryLimit, PaidUser = true, Unlimited = false };

            // Setup HttpRequest
            HttpRequestMessage requestMessage =
                this.CreateRequest($"http://localhost:0/foo", HttpMethod.Get);
            requestMessage.Headers.TryAddWithoutValidation("Authorization", "Basic NzE5ODRiMWItYTlhYy00NTc0LWIzZjYtNDhmNzkzMTIxMTEwCg==");

            this.Throttle_ByToken_Overlimit_InDatabase(cacheObject, requestMessage, queryLimit, recordsInDatabase);
        }

        [Test]
        public void Throttle_ByToken_InvalidToken()
        {
            IApiThrottler apiThrottler = new ApiThrottler(null, null);

            string invalidGuid = "invalid guid";

            HttpRequestMessage requestMessage =
                this.CreateRequest($"http://localhost:0/foo?token={invalidGuid}", HttpMethod.Get);

            bool paidUser;
            Assert.AreEqual(ApiThrottlingResult.InvalidToken, apiThrottler.Throttle(requestMessage, out paidUser));

            requestMessage = this.CreateRequest($"http://localhost:0/foo", HttpMethod.Get);
            requestMessage.Headers.TryAddWithoutValidation("Authorization", invalidGuid);

            Assert.AreEqual(ApiThrottlingResult.InvalidToken, apiThrottler.Throttle(requestMessage, out paidUser));
        }

        [Test]
        public void Throttle_ByToken_NonExistingToken()
        {
            Mock<IMorpherDatabase> databaseMock = new Mock<IMorpherDatabase>();
            databaseMock.Setup(morpherDatabase => morpherDatabase.GetUserLimits(It.IsAny<Guid>()))
                .Returns((CacheObject)null);

            IApiThrottler apiThrottler = new ApiThrottler(databaseMock.Object, new MorpherCache("TestCache"));
            Guid guid = Guid.NewGuid();
            HttpRequestMessage requestMessage = this.CreateRequest($"http://localhost:0/foo?token={guid}", HttpMethod.Get);

            bool paidUser;
            ApiThrottlingResult apiThrottlingResult = apiThrottler.Throttle(requestMessage, out paidUser);

            Assert.AreEqual(ApiThrottlingResult.InvalidToken, apiThrottlingResult);

        }

        [Test]
        public void Throttle_ByToken_FromQueryString_Unlimited()
        {
            int queryLimit = 1000;
            int recordsInDatabase = 5;

            // Setup database
            Mock<IMorpherDatabase> databaseMock = new Mock<IMorpherDatabase>();

            databaseMock.Setup(database => database.GetQueryCountByToken(It.IsAny<Guid>())).Returns(recordsInDatabase);
            databaseMock.Setup(database => database.GetUserLimits(It.IsAny<Guid>())).Returns(
                new CacheObject() { DailyLimit = queryLimit, PaidUser = true, Unlimited = true });

            // Setup ApiThrottler
            IApiThrottler apiThrottler = new ApiThrottler(databaseMock.Object, new MorpherCache("TestCache"));

            Guid guid = Guid.NewGuid();

            // Setup HttpRequest
            HttpRequestMessage requestMessage =
                this.CreateRequest($"http://localhost:0/foo?token={guid}", HttpMethod.Get);

            // Act
            int success = 0;
            for (int i = 0; i < queryLimit - recordsInDatabase; i++)
            {
                bool paidUser;
                ApiThrottlingResult apiThrottlingResult = apiThrottler.Throttle(requestMessage, out paidUser);

                if (apiThrottlingResult == ApiThrottlingResult.Success)
                {
                    success++;
                }
            }

            Assert.AreEqual(queryLimit - recordsInDatabase, success);
            Assert.AreEqual(true, apiThrottler.GetQueryLimit(guid).PaidUser);
            Assert.AreEqual(queryLimit, apiThrottler.GetQueryLimit(guid).DailyLimit);
        }

        [Test]
        public void RemoveFromCache()
        {
            Mock<IMorpherDatabase> databaseMock = new Mock<IMorpherDatabase>();
            databaseMock.Setup(database => database.GetUserLimits(It.IsAny<Guid>())).Returns(
                new CacheObject() { DailyLimit = 1000, PaidUser = true, Unlimited = true });

            IApiThrottler apiThrottler = new ApiThrottler(databaseMock.Object, new MorpherCache("Test"));
            Guid guid = Guid.NewGuid();
            apiThrottler.GetQueryLimit(guid);

            object obj = apiThrottler.RemoveFromCache(guid.ToString().ToLowerInvariant());

            Assert.NotNull(obj);
        }

        // NOT TESTS
        public void Throttle_ByToken_Overlimit_InDatabase(
            CacheObject cacheObject,
            HttpRequestMessage requestMessage,
            int queryLimit,
            int recordsInDatabase)
        {
            // Setup database
            Mock<IMorpherDatabase> databaseMock = new Mock<IMorpherDatabase>();

            databaseMock.Setup(database => database.GetQueryCountByToken(It.IsAny<Guid>())).Returns(recordsInDatabase);
            databaseMock.Setup(database => database.GetUserLimits(It.IsAny<Guid>())).Returns(cacheObject);

            // Setup ApiThrottler
            IApiThrottler apiThrottler = new ApiThrottler(databaseMock.Object, new MorpherCache("TestCache"));

            Guid guid = Guid.NewGuid();

            // Act
            bool paidUser;
            ApiThrottlingResult apiThrottlingResult = apiThrottler.Throttle(requestMessage, out paidUser);

            Assert.AreEqual(0, apiThrottler.GetQueryLimit(guid).DailyLimit);
            Assert.AreEqual(true, apiThrottler.GetQueryLimit(guid).PaidUser);
            Assert.AreEqual(ApiThrottlingResult.Overlimit, apiThrottlingResult);
        }

        public void Throttle_ByToken(
            CacheObject cacheObject,
            HttpRequestMessage requestMessage,
            int queryLimit,
            int overlimitAttempts,
            int recordsInDatabase)
        {
            // Setup database
            Mock<IMorpherDatabase> databaseMock = new Mock<IMorpherDatabase>();

            databaseMock.Setup(database => database.GetQueryCountByToken(It.IsAny<Guid>())).Returns(recordsInDatabase);
            databaseMock.Setup(database => database.GetUserLimits(It.IsAny<Guid>())).Returns(cacheObject);

            // Setup ApiThrottler
            IApiThrottler apiThrottler = new ApiThrottler(databaseMock.Object, new MorpherCache("TestCache"));

            Guid guid = Guid.NewGuid();

            // Act
            bool paidUser;
            int success = 0;
            for (int i = 0; i < queryLimit - recordsInDatabase; i++)
            {
                ApiThrottlingResult apiThrottlingResult = apiThrottler.Throttle(requestMessage, out paidUser);

                if (apiThrottlingResult == ApiThrottlingResult.Success)
                {
                    success++;
                }
            }

            Assert.AreEqual(queryLimit - recordsInDatabase, success);
            Assert.AreEqual(true, apiThrottler.GetQueryLimit(guid).PaidUser);
            Assert.AreEqual(0, apiThrottler.GetQueryLimit(guid).DailyLimit);

            int overlimit = 0;
            for (int i = 0; i < overlimitAttempts; i++)
            {
                ApiThrottlingResult apiThrottlingResult = apiThrottler.Throttle(requestMessage, out paidUser);

                if (apiThrottlingResult == ApiThrottlingResult.Overlimit)
                {
                    overlimit++;
                }
            }

            Assert.AreEqual(0, apiThrottler.GetQueryLimit(guid).DailyLimit);
            Assert.AreEqual(true, apiThrottler.GetQueryLimit(guid).PaidUser);
            Assert.AreEqual(overlimitAttempts, overlimit);
        }

        private HttpRequestMessage CreateRequest(string url, HttpMethod method)
        {
            var request = new HttpRequestMessage();

            var baseRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);
            var baseContext = new Mock<HttpContextBase>(MockBehavior.Strict);

            baseRequest.Setup(br => br.UserHostAddress).Returns("::1");
            baseContext.Setup(bc => bc.Request).Returns(baseRequest.Object);

            request.RequestUri = new Uri(url);

            request.Properties.Add("MS_HttpContext", baseContext.Object);

            request.Method = method;
            return request;
        }
    }
}