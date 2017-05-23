namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.MorpherSoap;
    using Morpher.WebService.V3.Services.Interfaces;

    public class UkrainianWebAnalyzer : IUkrainianAnalyzer
    {
        private readonly IUserCorrection userCorrection;

        private readonly Credentials credentials;

        public UkrainianWebAnalyzer(IUserCorrection userCorrection)
        {
            NameValueCollection conf = (NameValueCollection)ConfigurationManager.GetSection("WebServiceSettings");
            this.credentials = new Credentials()
                                   {
                                       Username = conf["WebServiceV2Login"],
                                       Password = conf["WebServiceV2Password"]
                                   };

            this.userCorrection = userCorrection;
        }

        public UkrainianDeclensionResult Declension(string s, Guid? token = null, DeclensionFlags? flags = null, bool paidUser = false)
        {
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