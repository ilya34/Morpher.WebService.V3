using System;
using System.Collections.Generic;
using System.Linq;
using Morpher.WebService.V3.General;
using Morpher.WebService.V3.Russian.Data;

namespace Morpher.WebService.V3.Russian.Data
{
    public class WebExceptionDictionary : IExceptionDictionary
    {
        private readonly Client _client;

        public WebExceptionDictionary(MorpherClient client)
        {
            _client = client.Russian;
        }

        public void Add(CorrectionPostModel model)
        {
            CorrectionForms singular = new CorrectionForms()
            {
                Nominative = model.И,
                Accusative = model.В,
                Dative = model.Д,
                Genitive = model.Р,
                Instrumental = model.Т,
                Locative = model.М,
                Prepositional = model.П
            };
            CorrectionForms plural = new CorrectionForms()
            {
                Nominative = model.М_И,
                Accusative = model.М_В,
                Dative = model.М_Д,
                Genitive = model.М_Р,
                Instrumental = model.М_Т,
                Locative = model.М_М,
                Prepositional = model.М_П
            };

            try
            {
                _client.UserDict.AddOrUpdate(new CorrectionEntry() {Singular = singular, Plural = plural});
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