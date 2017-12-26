﻿namespace Morpher.WebService.V3.Ukrainian.Data
{
    using System.Collections.Generic;
    using Data;

    public interface IExceptionDictionary
    {
        void Add(CorrectionPostModel model);
        bool Remove(string nominativeForm);
        List<Entry> GetAll();
    }
}
