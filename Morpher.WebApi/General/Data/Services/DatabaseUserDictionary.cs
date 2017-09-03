namespace Morpher.WebService.V3.General.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Web;
    using Interfaces;
    using Models;
    using Russian.Data;
    using DeclensionForms = Russian.Data.DeclensionForms;

    public class DatabaseUserDictionary : IUserDictionaryLookup, IExceptionDictionary
    {
        private readonly IMorpherCache _morpherCache;
        private readonly ICorrectionCache _correctionCache;

        public DatabaseUserDictionary(IMorpherCache morpherCache, ICorrectionCache correctionCache)
        {
            _morpherCache = morpherCache;
            _correctionCache = correctionCache;
        }

        private void LoadToCache(Guid userId)
        {
            using (UserCorrectionDataContext context = new UserCorrectionDataContext())
            {
                DataLoadOptions dataLoadOptions = new DataLoadOptions();
                dataLoadOptions.LoadWith<Name>(name => name.NameForms);
                context.LoadOptions = dataLoadOptions;
                var result = context.UserVotes.Where(vote => vote.UserID == userId)
                    .Select(vote => vote.Name).ToList();
                
                _correctionCache.Set(userId.ToString().ToLowerInvariant(), result,
                    new DateTimeOffset(DateTime.Today.AddDays(1)));
            }
        }

        /// <summary>
        /// Получает пользовательское исправление из БД
        /// </summary>
        /// <param name="nominativeSingular">именительная форма, регистр не учитывается</param>
        /// <returns>Пользовательское исправление</returns>
        public object Lookup(string nominativeSingular)
        {
            var token = HttpContext.Current.Request.GetToken();
            if (token == null)
            {
                return null;
            }

            var cache = (MorpherCacheObject)_morpherCache.Get(token.ToString().ToLowerInvariant());

            var correctionCache = (List<Name>)_correctionCache.Get(cache.UserId.ToString().ToLowerInvariant());

            if (correctionCache != null)
            {
                var result = correctionCache.FirstOrDefault(
                    name => name.Lemma == LemmaNormalizer.Normalize(nominativeSingular)
                            && name.LanguageID == "RU")?.NameForms;

                if (result == null || !result.Any())
                {
                    return null;
                }

                return result.ToList();
            }

            // load to cache
            LoadToCache(cache.UserId.Value);
            return Lookup(nominativeSingular);
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
                _correctionCache.Remove(cache.UserId.ToString().ToLowerInvariant());
            }
        }

        public bool Remove(string nominativeForm)
        {
            var token = HttpContext.Current.Request.GetToken();
            if (token == null)
            {
                throw new TokenNotFoundExceptionException();
            }

            var cache = (MorpherCacheObject)_morpherCache.Get(token.ToString().ToLowerInvariant());

            _correctionCache.Remove(cache.UserId.ToString().ToLowerInvariant());

            using (UserCorrectionDataContext context = new UserCorrectionDataContext())
            {
                var query = (from name in context.Names
                             join userVote in context.UserVotes on name.ID equals userVote.NameID
                             where userVote.UserID == cache.UserId
                                   && name.Lemma == LemmaNormalizer.Normalize(nominativeForm)
                             select new { name, userVote }).FirstOrDefault();
                if (query == null)
                {
                    return false;
                }

                foreach (var form in query.name.NameForms)
                {
                    context.NameForms.DeleteOnSubmit(form);
                }

                context.UserVotes.DeleteOnSubmit(query.userVote);
                context.SubmitChanges();
                return true;
            }
        }

        public List<Entry> GetAll()
        {
            var token = HttpContext.Current.Request.GetToken();
            if (token == null)
            {
                return null;
            }

            var cache = (MorpherCacheObject)_morpherCache.Get(token.ToString().ToLowerInvariant());

            var correctionCache = (List<Name>)_correctionCache.Get(cache.UserId.ToString().ToLowerInvariant());

            if (correctionCache == null)
            {
                LoadToCache(cache.UserId.Value);
                return GetAll();
            }

            List<Entry> entries = new List<Entry>();

            foreach (var name in correctionCache)
            {
                var correction = name.NameForms;
                var entry = new Entry(
                    new DeclensionForms(correction.Where(form => !form.Plural).ToList()),
                    new DeclensionForms(correction.Where(form => form.Plural).ToList()));
                entries.Add(entry);
            }

            return entries;
        }
    }
}