﻿namespace Morpher.WebService.V3.Shared.Interfaces
{
    using System;

    using Morpher.WebService.V3.Shared.Models;

    public interface IUkrainianAnalyzer
    {
        UkrainianDeclensionResult Declension(string s, Guid? token = null, DeclensionFlags? flags = null, bool paidUser = false);

        UkrainianNumberSpelling Spell(int n, string unit);
    }
}
