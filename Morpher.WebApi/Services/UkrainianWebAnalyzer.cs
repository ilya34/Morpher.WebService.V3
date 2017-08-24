namespace Morpher.WebService.V3.Services
{
    using System;
    using Interfaces;
    using Models;
    using Models.Exceptions;

    public class UkrainianWebAnalyzer : IUkrainianAnalyzer
    {
        private readonly Ukrainian.Client _client;

        public UkrainianWebAnalyzer(Ukrainian.Client client)
        {
            _client = client;
        }

        public UkrainianDeclensionResult Declension(string s, DeclensionFlags? flags = null)
        {
            try
            {
                return new UkrainianDeclensionResult(_client.Parse(s));
            }
            catch (Exception exc)
            {
                throw new MorpherException(exc.Message, -1);
            }
        }

        public UkrainianNumberSpelling Spell(int n, string unit)
        {
            try
            {
                var result = _client.Spell((uint) n, unit);
                return new UkrainianNumberSpelling(
                    new UkrainianDeclensionForms(result.NumberDeclension),
                    new UkrainianDeclensionForms(result.UnitDeclension));
            }
            catch (Exception exc)
            {
                throw new MorpherException(exc.Message, -1);
            }
        }
    }
}