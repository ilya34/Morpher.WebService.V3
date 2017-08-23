namespace Morpher.WebService.V3.Services.Interfaces
{
    using System.Collections.Generic;
    using Models;

    public interface IUkrainianDictService
    {
        void AddOrUpdate(UkrainianEntry ukrainianEntry);

        UkrainianDeclensionForms Get(string nominativeSingular, bool plural);

        List<UkrainianEntry> GetAll();

        void Load(List<UkrainianEntry> entries);

        void Remove(string singularNominative);
    }
}