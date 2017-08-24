namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Interfaces;
    using Models;
    using Models.Exceptions;
    using Russian;
    using AdjectiveGenders = Models.AdjectiveGenders;
    using DeclensionFlags = Models.DeclensionFlags;

    public class RussianWebAnalyzer : IRussianAnalyzer
    {
        private readonly Client _client;

        public RussianWebAnalyzer(Russian.Client client)
        {
            _client = client;
        }

        public RussianDeclensionResult Declension(string s, DeclensionFlags? flags = null)
        {
            try
            {
                return new RussianDeclensionResult(_client.Parse(s, flags.ToServiceFlags()));
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
                var result = _client.Spell((uint) n, unit);
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
                return new AdjectiveGenders(_client.AdjectiveGenders(s));
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
    }
}