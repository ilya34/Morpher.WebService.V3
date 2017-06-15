namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Linq;

    using Morpher.WebService.V3.Shared.Interfaces;
    using Morpher.WebService.V3.Shared.Models;

    public class UserCorrectionService : IUserCorrection
    {
        private readonly IUserCorrectionSource correctionSource;

        public UserCorrectionService(IUserCorrectionSource correctionSource)
        {
            this.correctionSource = correctionSource;
        }

        public void SetUserDeclensions(RussianDeclensionForms paradigm, string lemma, bool plural, Guid? token)
        {
            var corrections = this.correctionSource.GetUserCorrections(token, lemma, "RU");
            if (corrections != null && corrections.Any())
            {
                foreach (var correction in corrections.Where(form => form.Plural == plural))
                {
                    switch (correction.Form)
                    {
                        case "И":
                            paradigm.Nominative = correction.Lemma;
                            break;
                        case "Р":
                            paradigm.Genitive = correction.Lemma;
                            break;
                        case "Д":
                            paradigm.Dative = correction.Lemma;
                            break;
                        case "В":
                            paradigm.Accusative = correction.Lemma;
                            break;
                        case "Т":
                            paradigm.Instrumental = correction.Lemma;
                            break;
                        case "П":
                            paradigm.Prepositional = correction.Lemma;
                            break;
                        case "М":
                            paradigm.PrepositionalWithPre = correction.Lemma;
                            break;
                    }
                }
            }
        }

        public void SetUserDeclensions(UkrainianDeclensionForms paradigm, string lemma, bool plural, Guid? token)
        {
            var corrections = this.correctionSource.GetUserCorrections(token, lemma, "UK");
            if (corrections != null)
            {
                foreach (var correction in corrections.Where(form => form.Plural == plural))
                {
                    switch (correction.Form)
                    {
                        case "Н":
                            paradigm.Nominative = correction.Lemma;
                            break;
                        case "Р":
                            paradigm.Genitive = correction.Lemma;
                            break;
                        case "Д":
                            paradigm.Dative = correction.Lemma;
                            break;
                        case "З":
                            paradigm.Accusative = correction.Lemma;
                            break;
                        case "О":
                            paradigm.Instrumental = correction.Lemma;
                            break;
                        case "М":
                            paradigm.Prepositional = correction.Lemma;
                            break;
                        case "К":
                            paradigm.Vocative = correction.Lemma;
                            break;
                    }
                }
            }
        }
    }
}