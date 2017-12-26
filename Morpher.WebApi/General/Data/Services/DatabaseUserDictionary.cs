namespace Morpher.WebService.V3.General
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Web;
    using Models;

    public class DatabaseUserDictionary : 
        Russian.Data.IUserDictionaryLookup,
        Russian.Data.IExceptionDictionary,
        Ukrainian.Data.IUserDictionaryLookup,
        Ukrainian.Data.IExceptionDictionary
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

        private void Add(string nominativeSingular, List<NameForm> forms, CorrectionLanguage language)
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
                            && vote.Name.LanguageID == language.ToDatabaseLanguage());

                Name name;
                if (userVote == null)
                {
                    var nameId = Guid.NewGuid();
                    name = new Name() { ID = nameId, LanguageID = language.ToDatabaseLanguage() };
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
            if (cache == null)
            {
                var cacheJson = _morpherCache.GetAllAsJson();
                throw new Exception($"Cant find user in cache. Cache: {cacheJson}");
            }

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

        Russian.Data.Entry Russian.Data.IUserDictionaryLookup.Lookup(string nominativaSingular)
        {
            var list = Lookup(nominativaSingular, CorrectionLanguage.Russian);
            if (list == null)
            {
                return null;
            }

            Russian.Data.DeclensionFormsForCorrection singular = new
                Russian.Data.DeclensionFormsForCorrection(list.Where(form => !form.Plural).ToList());
            Russian.Data.DeclensionFormsForCorrection plural = null;
            var pluralList = list.Where(form => form.Plural).ToList();
            if (pluralList.Any())
            {
                plural = new Russian.Data.DeclensionFormsForCorrection(pluralList);
            }

            return new Russian.Data.Entry(singular, plural); 
        }

        void Russian.Data.IExceptionDictionary.Add(Russian.Data.CorrectionPostModel correctionPostModel)
        {
            List<NameForm> forms = correctionPostModel.ToNameForms();
            if (forms.Count < 2)
            {
             throw  new RequiredParameterIsNotSpecifiedException(message: "Нужно указать хотя бы одну косвенную форму.");
            }

            Add(correctionPostModel.И, forms, CorrectionLanguage.Russian);
        }

        bool Russian.Data.IExceptionDictionary.Remove(string nomitiveSingular)
        {
            return Remove(nomitiveSingular, CorrectionLanguage.Russian);
        }

        List<Russian.Data.Entry> Russian.Data.IExceptionDictionary.GetAll()
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

        Ukrainian.Data.Entry Ukrainian.Data.IUserDictionaryLookup.Lookup(string nominativeSingular)
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

        void Ukrainian.Data.IExceptionDictionary.Add(Ukrainian.Data.CorrectionPostModel correctionPostModel)
        {
            List<NameForm> forms = correctionPostModel.ToNameForms();
            if (forms.Count < 2)
            {
                throw new RequiredParameterIsNotSpecifiedException(message: "Нужно указать хотя бы одну косвенную форму.");
            }

            Add(correctionPostModel.Н, forms, CorrectionLanguage.Ukrainian);
        }

        bool Ukrainian.Data.IExceptionDictionary.Remove(string nominativeSingular)
        {
            return Remove(nominativeSingular, CorrectionLanguage.Ukrainian);
        }

        List<Ukrainian.Data.Entry> Ukrainian.Data.IExceptionDictionary.GetAll()
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