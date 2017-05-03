namespace Morpher.WebApi.UnitTest
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;

    using Morpher.WebApi.Models;
    using Morpher.WebApi.Services.Interfaces;

    using NUnit.Framework;

    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class MorpherLogTests
    {
        [Test]
        public void DataInserationTest_1()
        {
            MockDatabaseLog log = new MockDatabaseLog();
            IMorpherLog morpherLog = new Services.MorpherLog(log);

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, "http://localhost:0/test?q1=1&q2=2");

            morpherLog.Log(message);
            morpherLog.Sync();
            

            LogEntity entity;
            log.Logs.TryDequeue(out entity);

            Assert.NotNull(entity);
            Assert.AreEqual("q1=1;q2=2", entity.QueryString);
            Assert.AreEqual("/test", entity.QuerySource);
        }

        [Test]
        public void DataInserationTest_2()
        {
            MockDatabaseLog log = new MockDatabaseLog();
            IMorpherLog morpherLog = new Services.MorpherLog(log);
            Guid guid = Guid.NewGuid();
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:0/test?q1=1&q2=2&token={guid}");

            morpherLog.Log(message);
            morpherLog.Sync();


            LogEntity entity;
            log.Logs.TryDequeue(out entity);

            Assert.NotNull(entity);
            Assert.AreEqual($"q1=1;q2=2;token={guid}", entity.QueryString);
            Assert.AreEqual("/test", entity.QuerySource);
            Assert.AreEqual(guid, entity.WebServiceToken);
        }

        [Test]
        public void DataInserationTest_3()
        {
            MockDatabaseLog log = new MockDatabaseLog();
            IMorpherLog morpherLog = new Services.MorpherLog(log);
            Guid guid = Guid.Parse("4736dff6-a539-4764-98a9-21d19dc1326d");
            string basic = "Basic NDczNmRmZjYtYTUzOS00NzY0LTk4YTktMjFkMTlkYzEzMjZkCg==";
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:0/test?q1=1&q2=2");

            message.Headers.TryAddWithoutValidation("Authorization", basic);

            morpherLog.Log(message);
            morpherLog.Sync();


            LogEntity entity;
            log.Logs.TryDequeue(out entity);

            Assert.NotNull(entity);
            Assert.AreEqual($"q1=1;q2=2", entity.QueryString);
            Assert.AreEqual("/test", entity.QuerySource);
            Assert.AreEqual(guid, entity.WebServiceToken);
        }

        [SuppressMessage("ReSharper", "StyleCop.SA1307")]
        [SuppressMessage("ReSharper", "StyleCop.SA1401")]
        public class MockDatabaseLog : IDatabaseLog
        {
            public ConcurrentQueue<LogEntity> Logs;

            public void Upload(ConcurrentQueue<LogEntity> logs)
            {
                this.Logs = logs;
            }
        }
    }
}
