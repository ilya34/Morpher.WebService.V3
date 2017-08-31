namespace Morpher.WebService.V3.General.Data
{
    using System.Collections.Generic;
    using Russian.Data;

    public interface IExceptionDictionary
    {
        void Add(CorrectionPostModel model);
        bool Remove(string nominativeForm);
        List<Entry> GetAll();
    }
}
