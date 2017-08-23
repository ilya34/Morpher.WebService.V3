namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Models;

    public class DatabaseUserDictionary : IUserDictionaryLookup, IExceptionDictionary
    {
        /// <summary>
        /// Получает пользовательское исправление из БД
        /// </summary>
        /// <param name="nominativeSingular">именительная форма, регистр не учитывается</param>
        /// <param name="token">token пользователя</param>
        /// <returns>Пользовательское исправление</returns>
        public RussianEntry Lookup(string nominativeSingular, Guid? token)
        {
            using (UserCorrectionDataContext context = new UserCorrectionDataContext())
            {
                var result = (from correction in context.NameForms
                             join names in context.Names on correction.NameID equals names.ID
                             join userVote in context.UserVotes on names.ID equals userVote.NameID
                             where userVote.UserID == token
                                   && nominativeSingular.ToUpperInvariant() == names.Lemma
                                   && correction.LanguageID == "RU"
                             select correction).ToList();
                RussianEntry entry = new RussianEntry(
                    new RussianDeclensionForms(result.Where(form => !form.Plural).ToList()), 
                    new RussianDeclensionForms(result.Where(form => form.Plural).ToList()));
                return entry;
            }
        }

        public void Add(RussianEntry entry)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(string nominativeForm)
        {
            throw new System.NotImplementedException();
        }

        public List<RussianEntry> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}