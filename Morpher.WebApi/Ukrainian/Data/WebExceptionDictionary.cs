using System;
using System.Collections.Generic;
using System.Linq;
using Morpher.WebService.V3.General;

namespace Morpher.WebService.V3.Ukrainian.Data
{
    public class WebExceptionDictionary : IExceptionDictionary
    {
        private readonly Client _client;

        public WebExceptionDictionary(MorpherClient client)
        {
            _client = client.Ukrainian;
        }

        public void Add(CorrectionPostModel model)
        {
            CorrectionForms singular = new CorrectionForms()
            {
                Nominative = model.Н,
                Accusative = model.З,
                Dative = model.Д,
                Genitive = model.Р,
                Instrumental = model.О,
                Vocative = model.К,
                Prepositional = model.М
            };

            try
            {
                _client.UserDict.AddOrUpdate(new CorrectionEntry() { Singular = singular});
            }
            catch (Exception exc)
            {
                throw MorpherHelper.MapClientExceptionIfPossible(exc);
            }
        }

        public bool Remove(string nominativeForm)
        {
            try
            {
                return _client.UserDict.Remove(nominativeForm);
            }
            catch (Exception exc)
            {
                throw MorpherHelper.MapClientExceptionIfPossible(exc);
            }
        }

        public List<Entry> GetAll()
        {
            try
            {
                return _client.UserDict.GetAll().Select(entry => new Entry(entry)).ToList();
            }
            catch (Exception exc)
            {
                throw MorpherHelper.MapClientExceptionIfPossible(exc);
            }
        }
    }
}