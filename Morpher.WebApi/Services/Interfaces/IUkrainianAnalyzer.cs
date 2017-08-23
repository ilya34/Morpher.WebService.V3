namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;
    using Models;

    public interface IUkrainianAnalyzer
    {
        UkrainianDeclensionResult Declension(string s, Guid? token = null, DeclensionFlags? flags = null, bool paidUser = false);

        UkrainianNumberSpelling Spell(int n, string unit);
    }
}
