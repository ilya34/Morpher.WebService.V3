namespace Morpher.WebApi.Analyzers
{
    using System;
    using System.Net;

    using Morpher.WebApi.Analyzers.Interfaces;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Exceptions;
    using Morpher.WebApi.MorpherSoap;

    public class UkrainianWebAnalyzer : IUkrainianAnalyzer
    {
        private readonly ICustomDeclensions customDeclensions;

        private readonly Credentials credentials =
            new Credentials() { Username = "srgFilenko", Password = "morpherCorgi" };

        public UkrainianWebAnalyzer(ICustomDeclensions customDeclensions)
        {
            this.customDeclensions = customDeclensions;
        }

        public UkrainianDeclensionResult Declension(string s, Guid? token = null, bool paidUser = false)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new RussianWordsNotFoundException();
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
                this.customDeclensions.SetUserDeclensions(declensionResult, s, false, token.Value);
            }

            if (!paidUser)
            {
                declensionResult.Gender = null;
            }

            return declensionResult;
        }

        public UkrainianNumberSpelling Spell(decimal n, string unit)
        {
            if (string.IsNullOrWhiteSpace(unit))
            {
                throw new RussianWordsNotFoundException();
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
                                       Nominative = result.n.Н,
                                       Genitive = result.n.Р,
                                       Dative = result.n.Д,
                                       Accusative = result.n.З,
                                       Instrumental = result.n.О,
                                       Prepositional = result.n.М,
                                       Vocative = result.n.К
                                   }
                       };
        }
    }
}