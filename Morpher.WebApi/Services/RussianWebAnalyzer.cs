namespace Morpher.WebApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;

    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Exceptions;
    using Morpher.WebApi.MorpherSoap;
    using Morpher.WebApi.Services.Interfaces;

    using AdjectiveGenders = Morpher.WebApi.Models.AdjectiveGenders;

    public class RussianWebAnalyzer : IRussianAnalyzer
    {
        private readonly ICustomDeclensions customDeclensions;

        private readonly Credentials credentials =
            new Credentials() { Username = "srgFilenko", Password = "morpherCorgi" };

        public RussianWebAnalyzer(ICustomDeclensions customDeclensions)
        {
            this.customDeclensions = customDeclensions;
        }

        public RussianDeclensionResult Declension(string s, Guid? token = null, bool paidUser = false)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new WordsNotFoundException();
            }

            try
            {
                GetXmlResult result;
                using (WebServiceSoapClient client = new WebServiceSoapClient("WebServiceSoap"))
                {
                    result = client.GetXml(this.credentials, s);
                }

                RussianDeclensionResult declensionResult =
                    new RussianDeclensionResult()
                    {
                        Nominative = result.И,
                        Genitive = result.Р,
                        Dative = result.Д,
                        Accusative = result.В,
                        Instrumental = result.Т,
                        Prepositional = result.П,
                        Locative = result.По,
                        Where = result.где,
                        To = result.куда,
                        From = result.откуда,
                        Gender = result.род
                    };

                if (result.множественное != null)
                {
                    RussianDeclensionForms declensionForms =
                        new RussianDeclensionForms()
                        {
                            Nominative = result.множественное.И,
                            Genitive = result.множественное.Р,
                            Dative = result.множественное.Д,
                            Accusative = result.множественное.В,
                            Instrumental = result.множественное.Т,
                            Prepositional = result.множественное.П,
                            Locative = result.множественное.По
                        };
                    declensionResult.Plural = declensionForms;
                }

                if (result.ФИО != null)
                {
                    FullName fullName = new FullName()
                    {
                        Name = result.ФИО.И,
                        Surname = result.ФИО.Ф,
                        Pantronymic = result.ФИО.О
                    };
                    declensionResult.FullName = fullName;
                }

                if (token.HasValue)
                {
                    this.customDeclensions.SetUserDeclensions(declensionResult, s, false, token.Value);
                    if (declensionResult.Plural != null)
                    {
                        this.customDeclensions.SetUserDeclensions(declensionResult.Plural, s, true, token.Value);
                    }
                }

                if (!paidUser)
                {
                    declensionResult.Locative = null;
                    declensionResult.Gender = null;
                    declensionResult.Where = null;
                    declensionResult.From = null;
                    declensionResult.To = null;
                    if (declensionResult.Plural != null)
                    {
                        declensionResult.Plural.Locative = null;
                    }
                }

                return declensionResult;
            }
            catch (FaultException exception)
            {
                // BEST EXCEPTION HANDLING EVER. 11/10
                if (exception.Message.Contains("Не найдено русских слов"))
                {
                    throw new WordsNotFoundException();
                }

                if (exception.Message.Contains("Склонение числительных, заданных строкой, не поддерживается."))
                {
                    throw new NumeralsDeclensionNotSupportedException();
                }

                throw new Exception(exception.Message);
            }
        }

        public RussianNumberSpelling Spell(decimal n, string unit)
        {
            if (string.IsNullOrWhiteSpace(unit))
            {
                throw new WordsNotFoundException();
            }

            PropisResult result;
            using (WebServiceSoapClient client = new WebServiceSoapClient("WebServiceSoap"))
            {
                result = client.Propis(this.credentials, Convert.ToUInt32(n), unit);
            }

            return new RussianNumberSpelling()
            {
                NumberDeclension =
                               new RussianDeclensionForms()
                               {
                                   Nominative = result.n.И,
                                   Genitive = result.n.Р,
                                   Dative = result.n.Д,
                                   Accusative = result.n.В,
                                   Instrumental = result.n.Т,
                                   Prepositional = result.n.П,
                                   Locative = result.n.По
                               },
                UnitDeclension =
                               new RussianDeclensionForms()
                               {
                                   Nominative = result.unit.И,
                                   Genitive = result.unit.Р,
                                   Dative = result.unit.Д,
                                   Accusative = result.unit.В,
                                   Instrumental = result.unit.Т,
                                   Prepositional = result.unit.П,
                                   Locative = result.unit.По
                               }
            };
        }

        public AdjectiveGenders AdjectiveGenders(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new WordsNotFoundException();
            }

            MorpherSoap.AdjectiveGenders result;
            using (WebServiceSoapClient client = new WebServiceSoapClient("WebServiceSoap"))
            {
                result = client.GetAdjectiveGenders(this.credentials, s);
            }

            return new AdjectiveGenders { Plural = result.plural, Feminie = result.feminine, Neuter = result.neuter };
        }

        public List<string> Adjectives(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new WordsNotFoundException();
            }

            using (WebServiceSoapClient client = new WebServiceSoapClient("WebServiceSoap"))
            {
                return client.GetAdjectives(this.credentials, s).ToList();
            }
        }
    }
}