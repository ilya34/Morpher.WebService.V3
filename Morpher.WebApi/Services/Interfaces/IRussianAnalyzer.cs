namespace Morpher.WebApi.Services.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Morpher.WebApi.Models;

    public interface IRussianAnalyzer
    {
        RussianDeclensionResult Declension(string s, Guid? token = null, bool paidUser = false);

        RussianNumberSpelling Spell(decimal n, string unit);

        AdjectiveGenders AdjectiveGenders(string s);

        List<string> Adjectives(string s);
    }
}
