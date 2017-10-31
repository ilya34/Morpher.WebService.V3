namespace Morpher.WebService.V3.General.Data
{
    using System.Collections.Generic;
    using Russian.Data;
    using AdjectiveGenders = Russian.Data.AdjectiveGenders;
    using DeclensionResult = Russian.Data.DeclensionResult;

    public interface IRussianAnalyzer
    {
        DeclensionResult Declension(string s, DeclensionFlags? flags = null);

        NumberSpelling Spell(decimal n, string unit);

        AdjectiveGenders AdjectiveGenders(string s);

        List<string> Adjectives(string s);

        string Accentizer(string text);
    }
}
