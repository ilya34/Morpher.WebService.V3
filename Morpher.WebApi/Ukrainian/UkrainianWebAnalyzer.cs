namespace Morpher.WebService.V3.Ukrainian
{
    using System;
    using Data;
    using General.Data;

    public class UkrainianWebAnalyzer : IUkrainianAnalyzer
    {
        private readonly Client _client;

        public UkrainianWebAnalyzer(Client client)
        {
            _client = client;
        }

        public Data.DeclensionResult Declension(string s, DeclensionFlags? flags = null)
        {
            try
            {
                return new Data.DeclensionResult(_client.Parse(s));
            }
            catch (Exception exc)
            {
                throw new MorpherException(exc.Message, -1);
            }
        }

        public NumberSpelling Spell(decimal n, string unit)
        {
            try
            {
                var result = _client.Spell((uint) n, unit);
                return new NumberSpelling(
                    new Data.DeclensionForms(result.NumberDeclension),
                    new Data.DeclensionForms(result.UnitDeclension));
            }
            catch (Exception exc)
            {
                throw new MorpherException(exc.Message, -1);
            }
        }
    }
}