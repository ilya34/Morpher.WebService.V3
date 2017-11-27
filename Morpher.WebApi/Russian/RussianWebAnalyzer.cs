namespace Morpher.WebService.V3.Russian
{
    using System;
    using System.Collections.Generic;
    using Data;
    using General.Data;

    public class RussianWebAnalyzer : IRussianAnalyzer
    {
        private readonly Client _client;

        public RussianWebAnalyzer(Client client)
        {
            _client = client;
        }

        public Data.DeclensionResult Declension(string s, General.Data.DeclensionFlags? flags = null)
        {
            try
            {
                return new Data.DeclensionResult(_client.Parse(s, flags.ToServiceFlags()));
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
                //TODO: привести к одному типу
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

        public Data.AdjectiveGenders AdjectiveGenders(string s)
        {
            try
            {
                return new Data.AdjectiveGenders(_client.AdjectiveGenders(s));
            }
            catch (Exception exc)
            {
                throw new MorpherException(exc.Message, -1);
            }
        }

        public List<string> Adjectives(string s)
        {
            try
            {
                return _client.Adjectivize(s);
            }
            catch (Exception exc)
            {
                throw new MorpherException(exc.Message, -1);
            }
        }

        public string Accentizer(string text)
        {
            throw new NotImplementedException();
        }
    }
}