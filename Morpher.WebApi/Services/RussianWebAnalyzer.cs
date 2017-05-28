namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using System.ServiceModel;

    using Morpher.WebSerivce.V3.Shared.Models;
    using Morpher.WebService.V3.Models.Exceptions;
    using Morpher.WebService.V3.MorpherSoap;
    using Morpher.WebService.V3.Services.Interfaces;

    using AdjectiveGenders = Morpher.WebSerivce.V3.Shared.Models.AdjectiveGenders;


    public class RussianWebAnalyzer : IRussianAnalyzer
    {
        private readonly IUserCorrection userCorrection;

        private readonly Credentials credentials;

        public RussianWebAnalyzer(IUserCorrection userCorrection)
        {
            NameValueCollection conf = (NameValueCollection)ConfigurationManager.GetSection("WebServiceSettings");
            this.credentials = new Credentials()
                                   {
                                       Username = conf["WebServiceV2Login"], Password = conf["WebServiceV2Password"]
                                   };

            this.userCorrection = userCorrection;
        }

        public RussianDeclensionResult Declension(string s, Guid? token = null, DeclensionFlags? flags = null, bool paidUser = false)
        {
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
                        PrepositionalWithPre = result.По,
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
                            PrepositionalWithPre = result.множественное.По
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
                    this.userCorrection.SetUserDeclensions(declensionResult, s, false, token.Value);
                    if (declensionResult.Plural != null)
                    {
                        this.userCorrection.SetUserDeclensions(declensionResult.Plural, s, true, token.Value);
                    }
                }

                if (!paidUser)
                {
                    declensionResult.PrepositionalWithPre = null;
                    declensionResult.Gender = null;
                    declensionResult.Where = null;
                    declensionResult.From = null;
                    declensionResult.To = null;
                    if (declensionResult.Plural != null)
                    {
                        declensionResult.Plural.PrepositionalWithPre = null;
                    }
                }

                return declensionResult;
            }
            catch (FaultException exception)
            {
                // BEST EXCEPTION HANDLING EVER. 11/10
                if (exception.Message.Contains("Не найдено русских слов"))
                {
                    throw new RussianWordsNotFound();
                }

                if (exception.Message.Contains("Склонение числительных, заданных строкой, не поддерживается."))
                {
                    throw new NumeralsDeclensionNotSupportedException();
                }

                throw new Exception(exception.Message);
            }
        }

        public RussianNumberSpelling Spell(int n, string unit)
        {
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
                                   PrepositionalWithPre = result.n.По
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
                                   PrepositionalWithPre = result.unit.По
                               }
            };
        }

        public AdjectiveGenders AdjectiveGenders(string s)
        {
            MorpherSoap.AdjectiveGenders result;
            using (WebServiceSoapClient client = new WebServiceSoapClient("WebServiceSoap"))
            {
                result = client.GetAdjectiveGenders(this.credentials, s);
            }

            return new AdjectiveGenders { Plural = result.plural, Feminie = result.feminine, Neuter = result.neuter };
        }

        public List<string> Adjectives(string s)
        {
            using (WebServiceSoapClient client = new WebServiceSoapClient("WebServiceSoap"))
            {
                return client.GetAdjectives(this.credentials, s).ToList();
            }
        }
    }
}