namespace Morpher.WebService.V3.Services.Interfaces
{
    using System.Collections.Generic;
    using Models;

    public interface IExceptionDictionary
    {
        void Add(RussianEntry entry);
        bool Remove(string nominativeForm);
        List<RussianEntry> GetAll();
    }
}
