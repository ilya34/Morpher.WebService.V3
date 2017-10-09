namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Web;
    using Interfaces;
    using Models;
    using Russian.Data;

    public class DatabaseUserDictionary : 
        Russian.IUserDictionaryLookup,
        Russian.IExceptionDictionary,
        Ukrainian.IUserDictionaryLookup,
        Ukrainian.IExceptionDictionary
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

        private void Add(string nominativeSingular, List<NameForm> forms)
        {
            var token = HttpContext.Current.Request.GetToken();
            if (token == null)
            {
                throw new TokenNotFoundException();
            }

            var cache = (MorpherCacheObject)_morpherCache.Get(token.ToString().ToLowerInvariant());

            string normalizedLemma = LemmaNormalizer.Normalize(nominativeSingular);

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

        private bool Remove(string nominativeSingular, CorrectionLanguage language)
        {
            var token = HttpContext.Current.Request.GetToken();
            if (token == null)
            {
                throw new TokenNotFoundException();
            }

            var cache = (MorpherCacheObject)_morpherCache.Get(token.ToString().ToLowerInvariant());

            _correctionCache.Remove(cache.UserId.ToString().ToLowerInvariant());

            using (UserCorrectionDataContext context = new UserCorrectionDataContext())
            {
                var query = (from name in context.Names
                             join userVote in context.UserVotes on name.ID equals userVote.NameID
                             where userVote.UserID == cache.UserId
                                   && name.Lemma == LemmaNormalizer.Normalize(nominativeSingular)
                                   && name.LanguageID == language.ToDatabaseLanguage()
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

        public List<Name> GetAll(CorrectionLanguage language)
        {
            var token = HttpContext.Current.Request.GetToken();
            if (token == null)
            {
                throw new TokenNotFoundException();
            }

            var cache = (MorpherCacheObject)_morpherCache.Get(token.ToString().ToLowerInvariant());

            var correctionCache = (List<Name>)_correctionCache.Get(cache.UserId.ToString().ToLowerInvariant());

            if (correctionCache == null)
            {
                LoadToCache(cache.UserId.Value);
                return GetAll(language);
            }

            return correctionCache.Where(name => name.LanguageID == language.ToDatabaseLanguage()).ToList();
        }

        private List<NameForm> Lookup(string nominativeSingular, CorrectionLanguage language)
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
                            && name.LanguageID == language.ToDatabaseLanguage())?.NameForms;

                if (result == null || !result.Any())
                {
                    return null;
                }

                return result.ToList();
            }

            // load to cache
            LoadToCache(cache.UserId.Value);
            return Lookup(nominativeSingular, language);
        }

        LookupEntry Russian.IUserDictionaryLookup.Lookup(string nomonativeSingular)
        {
            var list = Lookup(nomonativeSingular, CorrectionLanguage.Russian);
            if (list == null)
            {
                return null;
            }

            return new LookupEntry(list); 
        }

        void Russian.IExceptionDictionary.Add(Russian.Data.CorrectionPostModel correctionPostModel)
        {
            List<NameForm> forms = correctionPostModel;
            Add(correctionPostModel.И, forms);
        }

        bool Russian.IExceptionDictionary.Remove(string nomitiveSingular)
        {
            return Remove(nomitiveSingular, CorrectionLanguage.Russian);
        }

        List<Russian.Data.Entry> Russian.IExceptionDictionary.GetAll()
        {
            var correctionCache = GetAll(CorrectionLanguage.Russian);
            List<Russian.Data.Entry> entries = new List<Russian.Data.Entry>();

            foreach (var name in correctionCache)
            {
                var correction = name.NameForms;
                var entry = new Russian.Data.Entry(
                    new Russian.Data.DeclensionFormsForCorrection(correction.Where(form => !form.Plural).ToList()),
                    new Russian.Data.DeclensionFormsForCorrection(correction.Where(form => form.Plural).ToList()));
                entries.Add(entry);
            }

            return entries;
        }

        Ukrainian.Data.Entry Ukrainian.IUserDictionaryLookup.Lookup(string nominativeSingular)
        {
            var list = Lookup(nominativeSingular, CorrectionLanguage.Ukrainian);
            if (list == null)
            {
                return null;
            }

            Ukrainian.Data.Entry entry = new Ukrainian.Data.Entry();
            entry.Singular = new Ukrainian.Data.DeclensionForms()
            {
                Nominative = list.SingleOrDefault(form => form.FormID == 'Н' && !form.Plural)?.AccentedText,
                Genitive = list.SingleOrDefault(form => form.FormID == 'Р' && !form.Plural)?.AccentedText,
                Accusative = list.SingleOrDefault(form => form.FormID == 'З' && !form.Plural)?.AccentedText,
                Dative = list.SingleOrDefault(form => form.FormID == 'Д' && !form.Plural)?.AccentedText,
                Instrumental = list.SingleOrDefault(form => form.FormID == 'О' && !form.Plural)?.AccentedText,
                Prepositional = list.SingleOrDefault(form => form.FormID == 'М' && !form.Plural)?.AccentedText,
                Vocative = list.SingleOrDefault(form => form.FormID == 'К' && !form.Plural)?.AccentedText
            };
            
            return entry;
        }

        void Ukrainian.IExceptionDictionary.Add(Ukrainian.Data.CorrectionPostModel correctionPostModel)
        {
            List<NameForm> forms = correctionPostModel;
            Add(correctionPostModel.Н, forms);
        }

        bool Ukrainian.IExceptionDictionary.Remove(string nominativeSingular)
        {
            return Remove(nominativeSingular, CorrectionLanguage.Ukrainian);
        }

        List<Ukrainian.Data.Entry> Ukrainian.IExceptionDictionary.GetAll()
        {
            var correctionCache = GetAll(CorrectionLanguage.Ukrainian);
            List<Ukrainian.Data.Entry> entries = new List<Ukrainian.Data.Entry>();

            foreach (var name in correctionCache)
            {
                var correction = name.NameForms;
                var entry = new Ukrainian.Data.Entry(
                    new Ukrainian.Data.DeclensionForms(correction.Where(form => !form.Plural).ToList()));
                entries.Add(entry);
            }

            return entries;
        }
    }
}