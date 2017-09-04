namespace Morpher.WebService.V3.General.Data
{
    using Ukrainian.Data;
    using DeclensionResult = Ukrainian.Data.DeclensionResult;

    public interface IUkrainianAnalyzer
    {
        DeclensionResult Declension(string s, DeclensionFlags? flags = null);

        NumberSpelling Spell(decimal n, string unit);
    }
}
