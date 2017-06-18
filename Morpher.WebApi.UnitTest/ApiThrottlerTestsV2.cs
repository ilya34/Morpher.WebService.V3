namespace Morpher.WebService.V3.UnitTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Runtime.Caching;
    using System.Threading;
    using System.Web;

    using Moq;

    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services;
    using Morpher.WebService.V3.Services.Interfaces;

    using NUnit.Framework;

    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ApiThrottlerTestsV2
    {
        [Test]
        public void Throttle_ByIp()
        {
            int queryLimit = 10;
            int recordsInDatabase = 5;

            // Setup database
            Mock<IMorpherDatabase> databaseMock = new Mock<IMorpherDatabase>();
            databaseMock.Setup(database => database.IsIpBlocked(It.IsAny<string>())).Returns(false);
            databaseMock.Setup(database => database.GetDefaultDailyQueryLimit()).Returns(queryLimit);
            databaseMock.Setup(database => database.GetQueryCountByIp(It.IsAny<string>())).Returns(recordsInDatabase);

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
            Assert.AreEqual(0, apiThrottler.GetQueryLimit("::1").QueriesLeft);

            {
                ApiThrottlingResult apiThrottlingResult = apiThrottler.Throttle(requestMessage, out paidUser);
                Assert.AreEqual(ApiThrottlingResult.Overlimit, apiThrottlingResult);
            }
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
            int recordsInDatabase = 5;

            MorpherCacheObject morpherCacheObject = new MorpherCacheObject() { QueriesLeft = queryLimit, PaidUser = true, Unlimited = false };

            Guid guid = Guid.NewGuid();
            HttpRequestMessage requestMessage =
                this.CreateRequest($"http://localhost:0/foo?token={guid}", HttpMethod.Get);

            this.Throttle_ByToken(morpherCacheObject, requestMessage, guid, queryLimit, recordsInDatabase);
        }

        [Test]
        public void Throttle_ByToken_FromHeader_PaidUser()
        {
            int queryLimit = 10;
            int recordsInDatabase = 5;

            MorpherCacheObject morpherCacheObject = new MorpherCacheObject() { QueriesLeft = queryLimit, PaidUser = true, Unlimited = false };

            Guid guid = Guid.Parse("5397a048-3b9c-42ad-8dbe-f7ad50b3a0de");
            string base64 = "NTM5N2EwNDgtM2I5Yy00MmFkLThkYmUtZjdhZDUwYjNhMGRlCg==";
            HttpRequestMessage requestMessage =
                this.CreateRequest($"http://localhost:0/foo", HttpMethod.Get);
            requestMessage.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64}");

            this.Throttle_ByToken(morpherCacheObject, requestMessage, guid, queryLimit, recordsInDatabase);
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
                .Returns((MorpherCacheObject)null);

            IApiThrottler apiThrottler = new ApiThrottler(databaseMock.Object, new MorpherCache("TestCache"));
            Guid guid = Guid.NewGuid();
            HttpRequestMessage requestMessage = this.CreateRequest($"http://localhost:0/foo?token={guid}", HttpMethod.Get);

            bool paidUser;
            ApiThrottlingResult apiThrottlingResult = apiThrottler.Throttle(requestMessage, out paidUser);

            Assert.AreEqual(ApiThrottlingResult.TokenNotFound, apiThrottlingResult);

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
                new MorpherCacheObject() { QueriesLeft = queryLimit, PaidUser = true, Unlimited = true });

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
            Assert.AreEqual(queryLimit, apiThrottler.GetQueryLimit(guid).QueriesLeft);
        }

        [Test]
        public void RemoveFromCache()
        {
            Mock<IMorpherDatabase> databaseMock = new Mock<IMorpherDatabase>();
            databaseMock.Setup(database => database.GetUserLimits(It.IsAny<Guid>())).Returns(
                new MorpherCacheObject() { QueriesLeft = 1000, PaidUser = true, Unlimited = true });

            IApiThrottler apiThrottler = new ApiThrottler(databaseMock.Object, new MorpherCache("Test"));
            Guid guid = Guid.NewGuid();
            apiThrottler.GetQueryLimit(guid);

            object obj = apiThrottler.RemoveFromCache(guid.ToString().ToLowerInvariant());

            Assert.NotNull(obj);
        }
        
        [Test]
        public void CacheExpiration()
        {
            IMorpherCache morpherCache = new MorpherCacheMock(new MemoryCache("Teset"));

            Mock<IMorpherDatabase> databaseMock = new Mock<IMorpherDatabase>();
            databaseMock.Setup(database => database.GetDefaultDailyQueryLimit()).Returns(1000).Verifiable();
            databaseMock.Setup(database => database.GetQueryCountByIp(It.IsAny<string>())).Returns(0).Verifiable();
            databaseMock.Setup(database => database.IsIpBlocked(It.IsAny<string>())).Returns(false).Verifiable();

            IApiThrottler apiThrottler = new ApiThrottler(databaseMock.Object, morpherCache);

            apiThrottler.GetQueryLimit("::1");
            apiThrottler.GetQueryLimit("::1");
            Thread.Sleep(60);
            apiThrottler.GetQueryLimit("::1");

            databaseMock.Verify(database => database.GetDefaultDailyQueryLimit(), Times.Exactly(2));
            databaseMock.Verify(database => database.GetQueryCountByIp(It.IsAny<string>()), Times.Exactly(2));
            databaseMock.Verify(database => database.IsIpBlocked(It.IsAny<string>()), Times.Exactly(2));
        }

        // NOT TESTS
        public void Throttle_ByToken(
            MorpherCacheObject morpherCacheObject,
            HttpRequestMessage requestMessage,
            Guid guid,
            int queryLimit,
            int recordsInDatabase)
        {
            // Setup database
            Mock<IMorpherDatabase> databaseMock = new Mock<IMorpherDatabase>();

            databaseMock.Setup(database => database.GetQueryCountByToken(It.IsAny<Guid>())).Returns(recordsInDatabase);
            databaseMock.Setup(database => database.GetUserLimits(It.IsAny<Guid>())).Returns(morpherCacheObject);

            // Setup ApiThrottler
            IApiThrottler apiThrottler = new ApiThrottler(databaseMock.Object, new MorpherCache("TestCache"));

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
            Assert.AreEqual(0, apiThrottler.GetQueryLimit(guid).QueriesLeft);
            Assert.AreEqual(ApiThrottlingResult.Overlimit, apiThrottler.Throttle(requestMessage, out paidUser));
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

        internal class MorpherCacheMock : IMorpherCache
        {
            private readonly MemoryCache memoryCache;

            public MorpherCacheMock(MemoryCache memoryCache)
            {
                this.memoryCache = memoryCache;
            }

            public bool Decrement(MorpherCacheObject morpherCacheObject)
            {
                throw new NotImplementedException();
            }

            public object Get(string key, string regionName = null)
            {
                return this.memoryCache.Get(key);
            }

            public void Set(string key, object value, DateTimeOffset absoluteExpirationDateTime, string regionName = null)
            {
                this.memoryCache.Set(key, value, DateTimeOffset.UtcNow.AddMilliseconds(50));
            }

            public void Set(string key, object value, CacheItemPolicy cacheItemPolicy, string regioName = null)
            {
                throw new NotImplementedException();
            }

            public object Remove(string key, string regionName = null)
            {
                throw new NotImplementedException();
            }
        }
    }
}