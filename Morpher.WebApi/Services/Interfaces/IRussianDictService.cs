// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Services.Interfaces
{
    using System.Collections.Generic;

    using Morpher.WebService.V3.Shared.Models;

    public interface IRussianDictService
    {
        void Load(List<RussianEntry> entries);

        RussianDeclensionForms Get(string nominativeSingular, bool plural);

        List<RussianEntry> GetAll();

        void AddOrUpdate(RussianEntry russianEntry);

        void Remove(string singularNominative);
    }
}