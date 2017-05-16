namespace Morpher.WebService.V3.UnitTests
{
    using System;
    using System.Collections.Generic;

    using Moq;

    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Models.Interfaces;
    using Morpher.WebService.V3.Services;
    using Morpher.WebService.V3.Services.Interfaces;

    using NUnit.Framework;

    [TestFixture]
    public class UserCorrectionTests
    {
        [Test]
        public void RussianSetUserDeclensions()
        {
            Mock<UserCorrectionSource> mock = new Mock<UserCorrectionSource>("connectionString");


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

            IUserCorrection userCorrection = new UserCorrection(mock.Object);
            IRussianParadigm testParadigm = new RussianDeclensionForms();
            IRussianParadigm russianParadigm = new RussianDeclensionForms()
                                                   {
                                                       Nominative = "им",
                                                       Genitive = "рд",
                                                       Dative = "дт",
                                                       Accusative = "вн",
                                                       Instrumental = "тв",
                                                       Prepositional = "пр",
                                                       Locative = "м"
                                                   };
            userCorrection.SetUserDeclensions(testParadigm, "тест", false, Guid.NewGuid());
            
            Assert.AreEqual(testParadigm, russianParadigm);
        }

        [Test]
        public void RussianPluralTest()
        {
            Mock<UserCorrectionSource> mock = new Mock<UserCorrectionSource>("connectionString");
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

            IRussianParadigm testParadigm = new RussianDeclensionForms();
            IRussianParadigm russianParadigm = new RussianDeclensionForms()
                                                   {
                                                       Nominative = "тест"
                                                   };

            IUserCorrection userCorrection = new UserCorrection(mock.Object);
            userCorrection.SetUserDeclensions(testParadigm, "test", true, Guid.Empty);

            Assert.AreEqual(russianParadigm, testParadigm);
        }

        [Test]
        public void UkrainianSetUserDeclensions()
        {
            Mock<UserCorrectionSource> mock = new Mock<UserCorrectionSource>("connectionString");


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

            IUserCorrection userCorrection = new UserCorrection(mock.Object);

            IUkrainianParadigm testParadigm = new UkrainianDeclensionForms();

            IUkrainianParadigm ukrainianParadigm = new UkrainianDeclensionForms()
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
            Mock<UserCorrectionSource> mock = new Mock<UserCorrectionSource>("connectionString");
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

            IUkrainianParadigm testParadigm = new UkrainianDeclensionForms();
            IUkrainianParadigm ukrainianParadigm = new UkrainianDeclensionForms()
                                                   {
                                                       Nominative = "тест"
                                                   };

            IUserCorrection userCorrection = new UserCorrection(mock.Object);
            userCorrection.SetUserDeclensions(testParadigm, "test", true, Guid.Empty);

            Assert.AreEqual(ukrainianParadigm, testParadigm);
        }
    }
}
