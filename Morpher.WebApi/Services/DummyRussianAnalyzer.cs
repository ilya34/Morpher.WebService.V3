namespace Morpher.WebApi.Services
{
    using System;
    using System.Collections.Generic;

    using Morpher.WebApi.Models;
    using Morpher.WebApi.Services.Interfaces;

    public class DummyRussianAnalyzer : IRussianAnalyzer
    {
        public RussianDeclensionResult Declension(string s, Guid? token = null, bool paidUser = false)
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