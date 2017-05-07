namespace Morpher.WebApi.Services
{
    using System;

    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Exceptions;
    using Morpher.WebApi.MorpherSoap;
    using Morpher.WebApi.Services.Interfaces;

    public class UkrainianWebAnalyzer : IUkrainianAnalyzer
    {
        private readonly IUserCorrection userCorrection;

        private readonly Credentials credentials =
            new Credentials() { Username = "srgFilenko", Password = "morpherCorgi" };

        public UkrainianWebAnalyzer(IUserCorrection userCorrection)
        {
            this.userCorrection = userCorrection;
        }

        public UkrainianDeclensionResult Declension(string s, Guid? token = null, DeclensionFlags? flags = null, bool paidUser = false)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new WordsNotFoundException();
            }

            GetXmlUkrResult result;
            using (WebServiceSoapClient client = new WebServiceSoapClient("WebServiceSoap"))
            {
                result = client.GetXmlUkr(this.credentials, s);
            }

            UkrainianDeclensionResult declensionResult = new UkrainianDeclensionResult()
                                                             {
                                                                 Nominative = result.Н,
                                                                 Genitive = result.Р,
                                                                 Dative = result.Д,
                                                                 Accusative = result.З,
                                                                 Instrumental = result.О,
                                                                 Prepositional = result.М,
                                                                 Vocative = result.К,
                                                                 Gender = result.рід
                                                             };

            if (token.HasValue)
            {
                this.userCorrection.SetUserDeclensions(declensionResult, s, false, token.Value);
            }

            if (!paidUser)
            {
                declensionResult.Gender = null;
            }

            return declensionResult;
        }

        public UkrainianNumberSpelling Spell(int n, string unit)
        {
            if (string.IsNullOrWhiteSpace(unit))
            {
                throw new WordsNotFoundException();
            }

            PropisUkrResult result;
            using (WebServiceSoapClient client = new WebServiceSoapClient("WebServiceSoap"))
            {
                result = client.PropisUkr(this.credentials, Convert.ToUInt32(n), unit);
            }

            return new UkrainianNumberSpelling()
                       {
                           NumberDeclension =
                               new UkrainianDeclensionForms()
                                   {
                                       Nominative = result.n.Н,
                                       Genitive = result.n.Р,
                                       Dative = result.n.Д,
                                       Accusative = result.n.З,
                                       Instrumental = result.n.О,
                                       Prepositional = result.n.М,
                                       Vocative = result.n.К
                                   },
                           UnitDeclension =
                               new UkrainianDeclensionForms()
                                   {
                                       Nominative = result.unit.Н,
                                       Genitive = result.unit.Р,
                                       Dative = result.unit.Д,
                                       Accusative = result.unit.З,
                                       Instrumental = result.unit.О,
                                       Prepositional = result.unit.М,
                                       Vocative = result.unit.К
                                   }
                       };
        }
    }
}