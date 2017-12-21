using System;
using System.Collections.Generic;
using Morpher.WebService.V3.General.Data;
using Morpher.WebService.V3.Russian.Data;

namespace Morpher.WebService.V3.Russian
{
    public class WebAnalyzer : IMorpher, IAdjectivizer, IAccentizer
    {
        private readonly Client _client;

        public WebAnalyzer(MorpherClient client)
        {
            _client = client.Russian;
        }

        public Data.DeclensionResult Declension(string s, General.Data.DeclensionFlags? flags = null)
        {
            try
            {
                return new Data.DeclensionResult(
                    _client.Parse(s, flags.MapFlags()));
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
                NumberSpelling numberSpelling = new NumberSpelling(
                    new Data.DeclensionForms(result.NumberDeclension),
                    new Data.DeclensionForms(result.UnitDeclension));
                return numberSpelling;
            }
            catch (Exception exc)
            {
                throw MorpherHelper.MapClientExceptionIfPossible(exc);
            }
        }

        public Data.AdjectiveGenders AdjectiveGenders(string s)
        {
            try
            {
                var result = _client.AdjectiveGenders(s);
                return new Data.AdjectiveGenders(result);
            }
            catch (Exception exc)
            {
                throw MorpherHelper.MapClientExceptionIfPossible(exc);
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
                throw MorpherHelper.MapClientExceptionIfPossible(exc);
            }
        }

        public string Accentizer(string text)
        {
            try
            {
                return _client.AddStressMarks(text);
            }
            catch (Exception exc)
            {
                throw MorpherHelper.MapClientExceptionIfPossible(exc);
            }
        }
    }
}