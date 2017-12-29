using Morpher.WebService.V3.General.Data;

namespace Morpher.WebService.V3.Ukrainian.Data
{
    public interface IUkrainianAnalyzer
    {
        DeclensionResult Declension(string s, DeclensionFlags? flags = null);

        NumberSpelling Spell(decimal n, string unit);
    }
}
