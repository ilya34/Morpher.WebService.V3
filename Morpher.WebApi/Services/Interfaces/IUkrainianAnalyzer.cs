namespace Morpher.WebApi.Services.Interfaces
{
    using System;

    using Morpher.WebApi.Models;

    public interface IUkrainianAnalyzer
    {
        UkrainianDeclensionResult Declension(string s, Guid? token = null, bool paidUser = false);

        UkrainianNumberSpelling Spell(int n, string unit);
    }
}
