﻿namespace Morpher.WebService.V3.UnitTests
{
    using System;
    using System.Collections.Generic;

    using Moq;

    using Morpher.WebService.V3.Services;
    using Morpher.WebService.V3.Shared.Interfaces;
    using Morpher.WebService.V3.Shared.Models;

    using NUnit.Framework;

    [TestFixture]
    public class UserCorrectionTests
    {
        [Test]
        public void RussianSetUserDeclensions()
        {
            Mock<UserCorrectionSourceDatabase> mock = new Mock<UserCorrectionSourceDatabase>("connectionString");


            mock.Setup(source => source.GetUserCorrections(It.IsAny<Guid>(), It.IsAny<string>(), "RU"))
                .Returns(
                new List<NameForm>()
                        {
                            #region Data
                            new NameForm()
                                {
                                    AccentedText = "им",
                                    FormID = "И",
                                    LanguageID = "RU",
                                    Plural = false,
                                },
                            new NameForm()
                                {
                                    AccentedText = "им",
                                    LanguageID = "RU",
                                    Plural = true,
                                },
                            new NameForm()
                                {
                                    AccentedText = "рд",
                                    FormID = "Р",
                                    LanguageID = "RU",
                                    Plural = false,
                                },
                            new NameForm()
                                {
                                    AccentedText = "дт",
                                    FormID = "Д",
                                    LanguageID = "RU",
                                    Plural = false,
                                },
                            new NameForm()
                                {
                                    AccentedText = "вн",
                                    FormID = "В",
                                    LanguageID = "RU",
                                    Plural = false,
                                },
                            new NameForm()
                                {
                                    AccentedText = "тв",
                                    FormID = "Т",
                                    LanguageID = "RU",
                                    Plural = false,
                                },
                            new NameForm()
                                {
                                    AccentedText = "пр",
                                    FormID = "П",
                                    LanguageID = "RU",
                                    Plural = false,
                                },
                            new NameForm()
                                {
                                    AccentedText = "м",
                                    FormID = "М",
                                    LanguageID = "RU",
                                    Plural = false,
                                },
                            #endregion
                        });

            IUserCorrection userCorrection = new UserCorrectionService(mock.Object);
            var testParadigm = new RussianDeclensionForms();
            var russianParadigm = new RussianDeclensionForms()
                                                   {
                                                       Nominative = "им",
                                                       Genitive = "рд",
                                                       Dative = "дт",
                                                       Accusative = "вн",
                                                       Instrumental = "тв",
                                                       Prepositional = "пр",
                                                       PrepositionalWithPre = "м"
                                                   };
            userCorrection.SetUserDeclensions(testParadigm, "тест", false, Guid.NewGuid());
            
            Assert.AreEqual(testParadigm, russianParadigm);
        }

        [Test]
        public void RussianPluralTest()
        {
            Mock<UserCorrectionSourceDatabase> mock = new Mock<UserCorrectionSourceDatabase>("connectionString");
            mock.Setup(source => source.GetUserCorrections(It.IsAny<Guid>(), It.IsAny<string>(), "RU")).Returns(
                new List<NameForm>()
                    {
                        new NameForm()
                            {
                                Plural = true,
                                FormID = "И",
                                AccentedText = "тест",
                                LanguageID = "RU"
                            }
                    });

            var testParadigm = new RussianDeclensionForms();
            var russianParadigm = new RussianDeclensionForms()
                                                   {
                                                       Nominative = "тест"
                                                   };

            IUserCorrection userCorrection = new UserCorrectionService(mock.Object);
            userCorrection.SetUserDeclensions(testParadigm, "test", true, Guid.Empty);

            Assert.AreEqual(russianParadigm, testParadigm);
        }

        [Test]
        public void UkrainianSetUserDeclensions()
        {
            Mock<UserCorrectionSourceDatabase> mock = new Mock<UserCorrectionSourceDatabase>("connectionString");


            mock.Setup(source => source.GetUserCorrections(It.IsAny<Guid>(), It.IsAny<string>(), "UK"))
                .Returns(
                    new List<NameForm>()
                        {
                            #region Data
                            new NameForm()
                                {
                                    AccentedText = "им",
                                    FormID = "Н",
                                    LanguageID = "UK",
                                    Plural = false,
                                },
                            new NameForm()
                                {
                                    AccentedText = "им",
                                    LanguageID = "UK",
                                    Plural = true,
                                },
                            new NameForm()
                                {
                                    AccentedText = "рд",
                                    FormID = "Р",
                                    LanguageID = "UK",
                                    Plural = false,
                                },
                            new NameForm()
                                {
                                    AccentedText = "дт",
                                    FormID = "Д",
                                    LanguageID = "UK",
                                    Plural = false,
                                },
                            new NameForm()
                                {
                                    AccentedText = "вн",
                                    FormID = "З",
                                    LanguageID = "UK",
                                    Plural = false,
                                },
                            new NameForm()
                                {
                                    AccentedText = "тв",
                                    FormID = "О",
                                    LanguageID = "UK",
                                    Plural = false,
                                },
                            new NameForm()
                                {
                                    AccentedText = "пр",
                                    FormID = "М",
                                    LanguageID = "UK",
                                    Plural = false,
                                },
                            new NameForm()
                                {
                                    AccentedText = "к",
                                    FormID = "К",
                                    LanguageID = "UK",
                                    Plural = false,
                                },
                            #endregion
                        });

            IUserCorrection userCorrection = new UserCorrectionService(mock.Object);

            var testParadigm = new UkrainianDeclensionForms();

            var ukrainianParadigm = new UkrainianDeclensionForms()
                                                   {
                                                       Nominative = "им",
                                                       Genitive = "рд",
                                                       Dative = "дт",
                                                       Accusative = "вн",
                                                       Instrumental = "тв",
                                                       Prepositional = "пр",
                                                       Vocative = "к"
                                                   };
            userCorrection.SetUserDeclensions(testParadigm, "test", false, Guid.NewGuid());

            Assert.AreEqual(testParadigm, ukrainianParadigm);
        }

        [Test]
        public void UkrainianPluralTest()
        {
            Mock<UserCorrectionSourceDatabase> mock = new Mock<UserCorrectionSourceDatabase>("connectionString");
            mock.Setup(source => source.GetUserCorrections(It.IsAny<Guid>(), It.IsAny<string>(), "UK")).Returns(
                new List<NameForm>()
                    {
                        new NameForm()
                            {
                                Plural = true,
                                FormID = "Н",
                                AccentedText = "тест",
                                LanguageID = "UK"
                            }
                    });

            var testParadigm = new UkrainianDeclensionForms();
            var ukrainianParadigm = new UkrainianDeclensionForms()
                                                   {
                                                       Nominative = "тест"
                                                   };

            IUserCorrection userCorrection = new UserCorrectionService(mock.Object);
            userCorrection.SetUserDeclensions(testParadigm, "test", true, Guid.Empty);

            Assert.AreEqual(ukrainianParadigm, testParadigm);
        }
    }
}
