namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Interfaces;
    using Models;
    using Models.Exceptions;

    public class RussianWebAnalyzer : IRussianAnalyzer
    {
        private readonly MorpherClient _client;

        public RussianWebAnalyzer(MorpherClient client)
        {
            _client = client;
        }

        public RussianDeclensionResult Declension(string s, DeclensionFlags? flags = null)
        {
            try
            {
                return new RussianDeclensionResult(_client.Russian.Parse(s, flags.ToServiceFlags()));
            }
            catch (Exception exc)
            {
                throw new MorpherException(exc.Message, -1);
            }
        }

        public RussianNumberSpelling Spell(int n, string unit)
        {
            try
            {
                //TODO: привести к одному типу
                var result = _client.Russian.Spell((uint) n, unit);
                return new RussianNumberSpelling(
                    new RussianDeclensionForms(result.NumberDeclension),
                    new RussianDeclensionForms(result.UnitDeclension));
            }
            catch (Exception exc)
            {
                throw new MorpherException(exc.Message, -1);
            }
        }

        public AdjectiveGenders AdjectiveGenders(string s)
        {
            try
            {
                return new AdjectiveGenders(_client.Russian.AdjectiveGenders(s));
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
                return _client.Russian.Adjectivize(s);
            }
            catch (Exception exc)
            {
                throw new MorpherException(exc.Message, -1);
            }
        }
    }
}