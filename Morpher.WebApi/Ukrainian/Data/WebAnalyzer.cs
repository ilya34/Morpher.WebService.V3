namespace Morpher.WebService.V3.Ukrainian.Data
{
    using System;
    using Data;
    using General;

    public class WebAnalyzer : IUkrainianAnalyzer
    {
        private readonly Client _client;

        public WebAnalyzer(MorpherClient client)
        {
            _client = client.Ukrainian;
        }

        public Data.DeclensionResult Declension(string s, DeclensionFlags? flags = null)
        {
            try
            {
                return new Data.DeclensionResult(_client.Parse(s));
            }
            catch (Exception exc)
            {
                throw MorpherHelper.MapClientExceptionIfPossible(exc);
            }
        }

        public NumberSpelling Spell(decimal n, string unit)
        {
            try
            {
                var result = _client.Spell(n, unit);
                return new NumberSpelling(
                    new Data.DeclensionForms(result.NumberDeclension),
                    new Data.DeclensionForms(result.UnitDeclension));
            }
            catch (Exception exc)
            {
                throw MorpherHelper.MapClientExceptionIfPossible(exc);
            }
        }
    }
}