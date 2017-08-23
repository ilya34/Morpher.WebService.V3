namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IRussianAnalyzer
    {
        RussianDeclensionResult Declension(string s, Guid? token = null, DeclensionFlags? flags = null, bool paidUser = false);

        RussianNumberSpelling Spell(int n, string unit);

        AdjectiveGenders AdjectiveGenders(string s);

        List<string> Adjectives(string s);
    }
}
