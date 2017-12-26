using System;
using System.Collections.Generic;
using Morpher.WebService.V3.General;

namespace Morpher.WebService.V3.Russian.Data
{
    public class WebAnalyzer : IMorpher, IAdjectivizer, IAccentizer
    {
        private readonly Client _client;

        public WebAnalyzer(MorpherClient client)
        {
            _client = client.Russian;
        }

        public DeclensionResult Declension(string s, General.DeclensionFlags? flags = null)
        {
            try
            {
                return new DeclensionResult(
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
                    new DeclensionForms(result.NumberDeclension),
                    new DeclensionForms(result.UnitDeclension));
                return numberSpelling;
            }
            catch (Exception exc)
            {
                throw MorpherHelper.MapClientExceptionIfPossible(exc);
            }
        }

        public AdjectiveGenders AdjectiveGenders(string s)
        {
            try
            {
                var result = _client.AdjectiveGenders(s);
                return new AdjectiveGenders(result);
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