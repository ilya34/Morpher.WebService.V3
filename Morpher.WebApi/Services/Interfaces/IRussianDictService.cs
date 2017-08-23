namespace Morpher.WebService.V3.Services.Interfaces
{
    using System.Collections.Generic;
    using Models;

    public interface IRussianDictService
    {
        void Load(List<RussianEntry> entries);

        RussianDeclensionForms Get(string nominativeSingular, bool plural);

        List<RussianEntry> GetAll();

        void AddOrUpdate(RussianEntry russianEntry);

        void Remove(string singularNominative);
    }
}