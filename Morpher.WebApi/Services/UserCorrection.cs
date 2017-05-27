namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Linq;

    using Morpher.WebService.V3.Models.Interfaces;
    using Morpher.WebService.V3.Services.Interfaces;

    public class UserCorrection : IUserCorrection
    {
        private readonly IUserCorrectionSource correctionSource;

        public UserCorrection(IUserCorrectionSource correctionSource)
        {
            this.correctionSource = correctionSource;
        }

        public void SetUserDeclensions(IRussianParadigm paradigm, string lemma, bool plural, Guid token = new Guid())
        {
            var nameForms = this.correctionSource.GetUserCorrections(token, lemma, "RU");
            if (nameForms != null && nameForms.Any())
            {
                foreach (var nameForm in nameForms.Where(form => form.Plural == plural))
                {
                    switch (nameForm.FormID)
                    {
                        case "И":
                            paradigm.Nominative = nameForm.AccentedText;
                            break;
                        case "Р":
                            paradigm.Genitive = nameForm.AccentedText;
                            break;
                        case "Д":
                            paradigm.Dative = nameForm.AccentedText;
                            break;
                        case "В":
                            paradigm.Accusative = nameForm.AccentedText;
                            break;
                        case "Т":
                            paradigm.Instrumental = nameForm.AccentedText;
                            break;
                        case "П":
                            paradigm.Prepositional = nameForm.AccentedText;
                            break;
                        case "М":
                            paradigm.PrepositionalWithPre = nameForm.AccentedText;
                            break;
                    }
                }
            }
        }

        public void SetUserDeclensions(IUkrainianParadigm paradigm, string lemma, bool plural, Guid token = new Guid())
        {
            var nameForms = this.correctionSource.GetUserCorrections(token, lemma, "UK");
            if (nameForms != null)
            {
                foreach (var nameForm in nameForms.Where(form => form.Plural == plural))
                {
                    switch (nameForm.FormID)
                    {
                        case "Н":
                            paradigm.Nominative = nameForm.AccentedText;
                            break;
                        case "Р":
                            paradigm.Genitive = nameForm.AccentedText;
                            break;
                        case "Д":
                            paradigm.Dative = nameForm.AccentedText;
                            break;
                        case "З":
                            paradigm.Accusative = nameForm.AccentedText;
                            break;
                        case "О":
                            paradigm.Instrumental = nameForm.AccentedText;
                            break;
                        case "М":
                            paradigm.Prepositional = nameForm.AccentedText;
                            break;
                        case "К":
                            paradigm.Vocative = nameForm.AccentedText;
                            break;
                    }
                }
            }
        }
    }
}