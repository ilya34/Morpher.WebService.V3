namespace Morpher.WebService.V3.General.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Models;
    using Russian.Data;
    using DeclensionForms = Russian.Data.DeclensionForms;

    public class DatabaseUserDictionary : IUserDictionaryLookup, IExceptionDictionary
    {
        private readonly IMorpherCache _morpherCache;

        public DatabaseUserDictionary(IMorpherCache morpherCache)
        {
            _morpherCache = morpherCache;
        }

        /// <summary>
        /// Получает пользовательское исправление из БД
        /// </summary>
        /// <param name="nominativeSingular">именительная форма, регистр не учитывается</param>
        /// <returns>Пользовательское исправление</returns>
        public Entry Lookup(string nominativeSingular)
        {
            var token = HttpContext.Current.Request.GetToken();
            if (token == null)
            {
                return null;
            }

            var cache = (MorpherCacheObject)_morpherCache.Get(token.ToString().ToLowerInvariant());

            using (UserCorrectionDataContext context = new UserCorrectionDataContext())
            {
                var result = (from correction in context.NameForms
                              join names in context.Names on correction.NameID equals names.ID
                              join userVote in context.UserVotes on names.ID equals userVote.NameID
                              where userVote.UserID == cache.UserId
                                    && nominativeSingular.ToUpperInvariant() == names.Lemma
                                    && correction.LanguageID == "RU"
                              select correction).ToList();

                if (result.Count == 0)
                {
                    return null;
                }

                Entry entry = new Entry(
                    new DeclensionForms(result.Where(form => !form.Plural).ToList()),
                    new DeclensionForms(result.Where(form => form.Plural).ToList()));
                return entry;
            }
        }

        public void Add(CorrectionPostModel model)
        {
            var token = HttpContext.Current.Request.GetToken();
            if (token == null)
            {
                throw new TokenNotFoundExceptionException();
            }

            var cache = (MorpherCacheObject)_morpherCache.Get(token.ToString().ToLowerInvariant());

            string normalizedLemma = LemmaNormalizer.Normalize(model.И);

            using (UserCorrectionDataContext context = new UserCorrectionDataContext())
            {
                var userVote = context.UserVotes.FirstOrDefault(
                    vote => vote.UserID == cache.UserId
                            && vote.Name.Lemma == normalizedLemma
                            && vote.Name.LanguageID == "RU");

                Name name;
                if (userVote == null)
                {
                    var nameId = Guid.NewGuid();
                    name = new Name() { ID = nameId, LanguageID = "RU" };
                    context.Names.InsertOnSubmit(name);
                    userVote = new UserVote() { Name = name, UserID = cache.UserId.Value };
                    context.UserVotes.InsertOnSubmit(userVote);
                }
                else
                {
                    name = userVote.Name;
                }

                name.Lemma = normalizedLemma;
                userVote.SubmittedUTC = DateTime.UtcNow;

                List<NameForm> forms = model;
                foreach (var nameForm in forms)
                {
                    var dbForm = name.NameForms.FirstOrDefault(
                        nf => nf.FormID == nameForm.FormID
                              && nf.Plural == nameForm.Plural);
                    if (dbForm == null)
                    {
                        nameForm.Name = name;
                        context.NameForms.InsertOnSubmit(nameForm);
                    }
                    else
                    {
                        dbForm.AccentedText = nameForm.AccentedText;
                    }
                }

                context.SubmitChanges();
            }
        }

        public bool Remove(string nominativeForm)
        {
            throw new System.NotImplementedException();
        }

        public List<Entry> GetAll()
        {
            var token = HttpContext.Current.Request.GetToken();
            if (token == null)
            {
                return null;
            }

            var cache = (MorpherCacheObject)_morpherCache.Get(token.ToString().ToLowerInvariant());

            List<Entry> entries = new List<Entry>();

            using (UserCorrectionDataContext context = new UserCorrectionDataContext())
            {
                var correctionIds = (from userVote in context.UserVotes
                                     where userVote.UserID == cache.UserId
                                     select userVote.NameID).ToList();

                foreach (var id in correctionIds)
                {
                    var correction = (from name in context.NameForms
                                      where name.NameID == id
                                      select name).ToList();
                    var entry = new Entry(
                        new DeclensionForms(correction.Where(form => !form.Plural).ToList()),
                        new DeclensionForms(correction.Where(form => form.Plural).ToList()));
                    entries.Add(entry);
                }
            }

            return entries;
        }
    }
}