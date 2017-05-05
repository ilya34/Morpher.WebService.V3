namespace Morpher.WebApi.Services
{
    using System;

    using Morpher.WebApi.Models;
    using Morpher.WebApi.Services.Interfaces;

    public class DummyUkrainianAnalyzer : IUkrainianAnalyzer
    {
        public UkrainianDeclensionResult Declension(string s, Guid? token = null, DeclensionFlags? flags = null, bool paidUser = false)
        {
            return new UkrainianDeclensionResult();
        }

        public UkrainianNumberSpelling Spell(int n, string unit)
        {
            return new UkrainianNumberSpelling();
        }
    }
}