namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IRussianAnalyzer
    {
        RussianDeclensionResult Declension(string s, DeclensionFlags? flags = null);

        RussianNumberSpelling Spell(int n, string unit);

        AdjectiveGenders AdjectiveGenders(string s);

        List<string> Adjectives(string s);
    }
}
