// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Morpher.WebService.V3.Shared.Models;

    public interface IRussianAnalyzer
    {
        RussianDeclensionResult Declension(string s, Guid? token = null, DeclensionFlags? flags = null, bool paidUser = false);

        RussianNumberSpelling Spell(int n, string unit);

        AdjectiveGenders AdjectiveGenders(string s);

        List<string> Adjectives(string s);
    }
}
