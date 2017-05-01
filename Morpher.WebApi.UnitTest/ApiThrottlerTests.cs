namespace Morpher.WebApi.UnitTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Moq;

    using Morpher.WebApi.ApiThrottler;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Services;
    using Morpher.WebApi.Services.Interfaces;

    using NUnit.Framework;

    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ApiThrottlerTests
    {
        [Test]
        public void RemoveFromCache_Null()
        {
            IMorpherCache morpherCache =
                Mock.Of<IMorpherCache>(cache => cache.Remove(It.IsAny<string>(), null) == null);

            IApiThrottler apiThrottler = new ApiThrottler(null, morpherCache);

            object obj = apiThrottler.RemoveFromCache("test");

            Assert.Null(obj);
        }

        [Test]
        public void RemoveFromCache_NotNull()
        {
            IMorpherCache morpherCache =
                Mock.Of<IMorpherCache>(cache => cache.Remove(It.IsAny<string>(), null) == new object());

            IApiThrottler apiThrottler = new ApiThrottler(null, morpherCache);

            object obj = apiThrottler.RemoveFromCache("test");

            Assert.NotNull(obj);
        }

        [Test]
        public void GetQueryLimit_ByIp_Blocked()
        {
            IMorpherDatabase morpherDatabase =
                Mock.Of<IMorpherDatabase>(database => database.IsIpBlocked(It.IsAny<string>()));

            IMorpherCache morpherCache = Mock.Of<IMorpherCache>(cache => cache.Get(It.IsAny<string>(), null) == null);

            IApiThrottler apiThrottler = new ApiThrottler(morpherDatabase, morpherCache);

            CacheObject cacheObject = apiThrottler.GetQueryLimit("any ip");

            Assert.Null(cacheObject);
        }

        [Test]
        public void GetQueryLimit_ByIp_NotFoundInCache_Success()
        {
            Mock<IMorpherCache> mock = new Mock<IMorpherCache>();

            mock.Setup(cache => cache.Get(It.IsAny<string>(), null)).Returns(null);
            mock.Setup(cache => cache.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>(), null)).Verifiable();

            IMorpherDatabase morpherDatabase =
                Mock.Of<IMorpherDatabase>(database => database.IsIpBlocked(It.IsAny<string>()) == false
                && database.GetDefaultDailyQueryLimit() == 1000
                && database.GetQueryCountByIp(It.IsAny<string>()) == 0);

            IApiThrottler apiThrottler = new ApiThrottler(morpherDatabase, mock.Object);

            CacheObject cacheObject = apiThrottler.GetQueryLimit("::1");

            mock.Verify(cache => cache.Set("::1", It.IsAny<object>(), It.IsAny<DateTimeOffset>(), null));
            Assert.NotNull(cacheObject);
        }

        [Test]
        public void GetQueryLimit_ByIp_FoundInCache()
        {
            IMorpherCache morpherCache =
                Mock.Of<IMorpherCache>(cache => cache.Get(It.IsAny<string>(), null) == new CacheObject());

            IApiThrottler apiThrottler = new ApiThrottler(null, morpherCache);

            Assert.NotNull(apiThrottler.GetQueryLimit("any ip"));
        }

        [Test]
        public void GetQueryLimit_ByToken_FoundInCache()
        {
            IMorpherCache morpherCache =
                Mock.Of<IMorpherCache>(cache => cache.Get(It.IsAny<string>(), null) == new CacheObject());

            IApiThrottler apiThrottler = new ApiThrottler(null, morpherCache);

            Assert.NotNull(apiThrottler.GetQueryLimit(Guid.NewGuid()));
        }

        [Test]
        public void GetQueryLimit_ByToken_NonExistingToken()
        {
            IMorpherCache morpherCache = Mock.Of<IMorpherCache>(cache => cache.Get(It.IsAny<string>(), null) == null);
            Mock<IMorpherDatabase> morpherDatabaseMock = new Mock<IMorpherDatabase>();
            morpherDatabaseMock.Setup(database => database.GetUserLimits(It.IsAny<Guid>())).Returns(() => null);
            IMorpherDatabase morpherDatabase = morpherDatabaseMock.Object;

            ApiThrottler apiThrottler = new ApiThrottler(morpherDatabase, morpherCache);

            Assert.Null(apiThrottler.GetQueryLimit(Guid.NewGuid()));
        }

        [Test]
        public void GetQueryLimit_ByToken_FoundInDatabase_Unlimited()
        {
            Mock<IMorpherCache> morpherCacheMock = new Mock<IMorpherCache>();
            morpherCacheMock.Setup(cache => cache.Get(It.IsAny<string>(), null)).Returns(null).Verifiable();
            morpherCacheMock.Setup(
                cache => cache.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>(), null)).Verifiable();

            Mock<IMorpherDatabase> morpherDatabaseMock = new Mock<IMorpherDatabase>();
            morpherDatabaseMock.Setup(database => database.GetUserLimits(It.IsAny<Guid>()))
                .Returns(new CacheObject() { Unlimited = true });
            morpherDatabaseMock.Setup(database => database.GetQueryCountByToken(It.IsAny<Guid>())).Verifiable();

            IApiThrottler apiThrottler = new ApiThrottler(morpherDatabaseMock.Object, morpherCacheMock.Object);

            CacheObject cacheObject = apiThrottler.GetQueryLimit(Guid.NewGuid());

            morpherCacheMock.Verify(cache => cache.Get(It.IsAny<string>(), null), Times.Once);
            morpherCacheMock.Verify(
                cache => cache.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>(), null),
                Times.Once);

            morpherDatabaseMock.Verify(database => database.GetUserLimits(It.IsAny<Guid>()), Times.Once);
            morpherDatabaseMock.Verify(database => database.GetQueryCountByToken(It.IsAny<Guid>()), Times.Never);
            Assert.NotNull(cacheObject);
        }

        [Test]
        public void GetQueryLimit_ByToken_FoundInDatabase_Limited()
        {
            Mock<IMorpherCache> morpherCacheMock = new Mock<IMorpherCache>();
            morpherCacheMock.Setup(cache => cache.Get(It.IsAny<string>(), null)).Returns(null).Verifiable();
            morpherCacheMock.Setup(
                cache => cache.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>(), null)).Verifiable();

            Mock<IMorpherDatabase> morpherDatabaseMock = new Mock<IMorpherDatabase>();
            morpherDatabaseMock.Setup(database => database.GetUserLimits(It.IsAny<Guid>()))
                .Returns(new CacheObject() { Unlimited = false });
            morpherDatabaseMock.Setup(database => database.GetQueryCountByToken(It.IsAny<Guid>())).Returns(0).Verifiable();

            IApiThrottler apiThrottler = new ApiThrottler(morpherDatabaseMock.Object, morpherCacheMock.Object);

            CacheObject cacheObject = apiThrottler.GetQueryLimit(Guid.NewGuid());

            morpherCacheMock.Verify(cache => cache.Get(It.IsAny<string>(), null), Times.Once);
            morpherCacheMock.Verify(
                cache => cache.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>(), null),
                Times.Once);

            morpherDatabaseMock.Verify(database => database.GetUserLimits(It.IsAny<Guid>()), Times.Once);
            morpherDatabaseMock.Verify(database => database.GetQueryCountByToken(It.IsAny<Guid>()), Times.Once);
            Assert.NotNull(cacheObject);
        }

        [Test]
        public void Throttle_Ip_IpBlocked()
        {
            IMorpherDatabase morpherDatabase =
                Mock.Of<IMorpherDatabase>(database => database.IsIpBlocked(It.IsAny<string>()) == true);

            IMorpherCache cache = Mock.Of<IMorpherCache>(
                morpherCache => morpherCache.Get(It.IsAny<string>(), null) == null);

            IApiThrottler apiThrottler = new ApiThrottler(morpherDatabase, cache);

            ApiThrottlingResult result = apiThrottler.Throttle("any ip");

            Assert.AreEqual(ApiThrottlingResult.IpBlocked, result);
        }

        [Test]
        public void Throttle_Ip_Overlimit()
        {
            IMorpherCache morpherCache =
                Mock.Of<IMorpherCache>(cache => cache.Get(It.IsAny<string>(), null) == new CacheObject()
                {
                    DailyLimit = 0,
                    Unlimited = false
                }
                && cache.Decrement(It.IsAny<string>()) == false);

            IApiThrottler apiThrottler = new ApiThrottler(null, morpherCache);

            ApiThrottlingResult result = apiThrottler.Throttle("any ip");

            Assert.AreEqual(ApiThrottlingResult.Overlimit, result);
        }

        [Test]
        public void Throttle_Ip_Success()
        {
            IMorpherCache morpherCache =
                Mock.Of<IMorpherCache>(cache => cache.Get(It.IsAny<string>(), null) == new CacheObject()
                {
                    DailyLimit = 0,
                    Unlimited = false
                }
                && cache.Decrement(It.IsAny<string>()) == true);

            IApiThrottler apiThrottler = new ApiThrottler(null, morpherCache);

            ApiThrottlingResult result = apiThrottler.Throttle("any ip");

            Assert.AreEqual(ApiThrottlingResult.Success, result);
        }

        [Test]
        public void Throttle_Token_Success_Limited()
        {
            Mock<IMorpherCache> morpherCacheMock = new Mock<IMorpherCache>();
            morpherCacheMock.Setup(
                cache => cache.Get(It.IsAny<string>(), null)).Returns(new CacheObject() { Unlimited = false });
            morpherCacheMock.Setup(cache => cache.Decrement(It.IsAny<string>())).Returns(true).Verifiable();

            IApiThrottler apiThrottler = new ApiThrottler(null, morpherCacheMock.Object);
            bool paidUser;
            ApiThrottlingResult result = apiThrottler.Throttle(Guid.NewGuid(), out paidUser);

            morpherCacheMock.Verify(cache => cache.Decrement(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(ApiThrottlingResult.Success, result);
        }

        [Test]
        public void Throttle_Token_Success_Unlimited()
        {
            Mock<IMorpherCache> morpherCacheMock = new Mock<IMorpherCache>();
            morpherCacheMock.Setup(
                cache => cache.Get(It.IsAny<string>(), null)).Returns(new CacheObject() { Unlimited = true });
            morpherCacheMock.Setup(cache => cache.Decrement(It.IsAny<string>())).Returns(true).Verifiable();

            IApiThrottler apiThrottler = new ApiThrottler(null, morpherCacheMock.Object);
            bool paidUser;
            ApiThrottlingResult result = apiThrottler.Throttle(Guid.NewGuid(), out paidUser);

            morpherCacheMock.Verify(cache => cache.Decrement(It.IsAny<string>()), Times.Never);
            Assert.AreEqual(ApiThrottlingResult.Success, result);
        }

        [Test]
        public void Throttle_Token_Overlimit()
        {
            Mock<IMorpherCache> morpherCacheMock = new Mock<IMorpherCache>();
            morpherCacheMock.Setup(
                cache => cache.Get(It.IsAny<string>(), null)).Returns(new CacheObject() { Unlimited = false });
            morpherCacheMock.Setup(cache => cache.Decrement(It.IsAny<string>())).Returns(false).Verifiable();

            IApiThrottler apiThrottler = new ApiThrottler(null, morpherCacheMock.Object);
            bool paidUser;
            ApiThrottlingResult result = apiThrottler.Throttle(Guid.NewGuid(), out paidUser);

            morpherCacheMock.Verify(cache => cache.Decrement(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(ApiThrottlingResult.Overlimit, result);
        }

        [Test]
        public void Throttle_Token_InvalidToken()
        {
            IMorpherCache morpherCache = Mock.Of<IMorpherCache>(cache => cache.Get(It.IsAny<string>(), null) == null);
            Mock<IMorpherDatabase> morpherDatabaseMock = new Mock<IMorpherDatabase>();
            morpherDatabaseMock.Setup(database => database.GetUserLimits(It.IsAny<Guid>())).Returns(() => null);
            IMorpherDatabase morpherDatabase = morpherDatabaseMock.Object;

            IApiThrottler apiThrottler = new ApiThrottler(morpherDatabase, morpherCache);
            bool paidUser;
            ApiThrottlingResult result = apiThrottler.Throttle(Guid.NewGuid(), out paidUser);

            Assert.AreEqual(ApiThrottlingResult.InvalidToken, result);
        }
    }
}
