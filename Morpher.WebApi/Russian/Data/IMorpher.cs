namespace Morpher.WebService.V3.Russian.Data
{
    public interface IMorpher
    {
        DeclensionResult Declension(string s, General.DeclensionFlags? flags = null);
        NumberSpelling Spell(decimal n, string unit);
        AdjectiveGenders AdjectiveGenders(string s);
    }
}