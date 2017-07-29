// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Services
{
    using System;

    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Interfaces;
    using Morpher.WebService.V3.Shared.Models;

    public class UserCorrectionFileService : IUserCorrection
    {
        private readonly IRussianDictService russianDictService;

        private readonly IUkrainianDictService ukrainianDictService;

        public UserCorrectionFileService(IRussianDictService russianDictService, IUkrainianDictService ukrainianDictService)
        {
            this.russianDictService = russianDictService;
            this.ukrainianDictService = ukrainianDictService;
        }

        public void SetUserDeclensions(RussianDeclensionForms paradigm, string lemma, bool plural, Guid? token)
        {
            var correctedForms = this.russianDictService.Get(lemma.ToUpperInvariant(), plural);
            if (correctedForms == null)
            {
                return;
            }

            if (!plural)
            {
                correctedForms.Nominative = null;
            }

            paradigm.AddOrUpdate(correctedForms);
        }

        public void SetUserDeclensions(UkrainianDeclensionForms paradigm, string lemma, bool plural, Guid? token)
        {
            var correctedForms = this.ukrainianDictService.Get(lemma.ToUpperInvariant(), plural);
            if (correctedForms == null)
            {
                return;
            }

            if (!plural)
            {
                correctedForms.Nominative = null;
            }

            correctedForms.Nominative = null;
            paradigm.AddOrUpdate(correctedForms);
        }
    }
}