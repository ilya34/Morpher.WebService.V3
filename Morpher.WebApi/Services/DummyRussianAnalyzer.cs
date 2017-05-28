namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Generic;

    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Interfaces;
    using Morpher.WebService.V3.Shared.Models;

    public class DummyRussianAnalyzer : IRussianAnalyzer
    {
        public RussianDeclensionResult Declension(string s, Guid? token = null, DeclensionFlags? flags = null, bool paidUser = false)
        {
            return new RussianDeclensionResult();
        }

        public RussianNumberSpelling Spell(int n, string unit)
        {
            return new RussianNumberSpelling();
        }

        public AdjectiveGenders AdjectiveGenders(string s)
        {
            return new AdjectiveGenders();
        }

        public List<string> Adjectives(string s)
        {
            return new List<string>();
        }
    }
}