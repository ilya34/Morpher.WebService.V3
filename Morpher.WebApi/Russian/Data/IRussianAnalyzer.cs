using System.Collections.Generic;

namespace Morpher.WebService.V3.Russian.Data
{
    public interface IRussianAnalyzer
    {
        DeclensionResult Declension(string s, General.Data.DeclensionFlags? flags = null);

        NumberSpelling Spell(decimal n, string unit);

        AdjectiveGenders AdjectiveGenders(string s);

        List<string> Adjectives(string s);

        string Accentizer(string text);
    }
}
