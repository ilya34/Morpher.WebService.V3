namespace Morpher.WebService.V3.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Text;

    using Morpher.WebService.V3.Services;
    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Models;

    using Newtonsoft.Json;

    using NUnit.Framework;

    [TestFixture]
    public class UserCorrectionSourceFileTests
    {
        [Test]
        public void GetUserCorrections()
        {
            IMorpherCache morpherCache = new MorpherCache("Test");
            
            Correction correction = new Correction() { Lemma = "Тест", Form = "Д", Plural = false };
            List<Correction> corrections = new List<Correction>() { correction };
            UserCorrectionEntity entity =
                new UserCorrectionEntity() { Language = "RU", NominativeForm = "ТЕСТ", Corrections = corrections };
            List<UserCorrectionEntity> entities = new List<UserCorrectionEntity>() { entity };
            morpherCache.Set("local", entities, new CacheItemPolicy());

            UserCorrectionSourceFile correctionSourceFile = new UserCorrectionSourceFile(morpherCache);

            var goodList = correctionSourceFile.GetUserCorrections(null, "Тест", "RU");
            var nullList = correctionSourceFile.GetUserCorrections(null, "Test", "RU");
            var nullList2 = correctionSourceFile.GetUserCorrections(null, "Тест", "UK");

            Assert.AreEqual(1, goodList.Count);
            Assert.AreEqual(correction, goodList.First());
            Assert.Null(nullList);
            Assert.Null(nullList2);
        }

        [Test]
        public void AssignNewCorrection_NewCorrection()
        {
            IMorpherCache morpherCache = new MorpherCache("Test");

            Correction correction = new Correction() { Lemma = "Тест", Form = "Д", Plural = false };
            List<Correction> corrections = new List<Correction>() { correction };
            UserCorrectionEntity entity =
                new UserCorrectionEntity() { Language = "RU", NominativeForm = "ТЕСТ", Corrections = corrections };
            List<UserCorrectionEntity> entities = new List<UserCorrectionEntity>();
            morpherCache.Set("local", entities, new CacheItemPolicy());

            UserCorrectionSourceFile correctionSourceFile = new UserCorrectionSourceFile(morpherCache);
            
            correctionSourceFile.AssignNewCorrection(null, entity);
            var list = correctionSourceFile.GetUserCorrections(null, "Тест", "RU");
            
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(correction, list.First());

            string json = File.ReadAllText($"{Directory.GetCurrentDirectory()}/UserDict.json", Encoding.UTF8);
            List<UserCorrectionEntity> fileEntities = JsonConvert.DeserializeObject<List<UserCorrectionEntity>>(json);

            Assert.AreEqual(1, fileEntities.Count);
            Assert.AreEqual(correction, fileEntities.First().Corrections.First());
        }

        [Test]
        public void AssignNewCorrection_AddOrUpdate()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void RemoveCorrection()
        {
            throw new NotImplementedException();
        }
    }
}
