namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;
    using Models;

    public interface IUkrainianAnalyzer
    {
        UkrainianDeclensionResult Declension(string s, DeclensionFlags? flags = null);

        UkrainianNumberSpelling Spell(int n, string unit);
    }
}
