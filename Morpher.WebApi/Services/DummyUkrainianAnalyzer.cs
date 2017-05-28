namespace Morpher.WebService.V3.Services
{
    using System;

    using Morpher.WebService.V3.Shared.Interfaces;
    using Morpher.WebService.V3.Shared.Models;

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