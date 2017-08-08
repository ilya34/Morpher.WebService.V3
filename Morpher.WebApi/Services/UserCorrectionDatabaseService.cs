namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Linq;

    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Shared.Interfaces;
    using Morpher.WebService.V3.Shared.Models;

    public class UserCorrectionDatabaseService : IUserCorrection
    {
        public void SetUserDeclensions(RussianDeclensionForms paradigm, string lemma, bool plural, Guid? token)
        {
            using (UserCorrectionDataContext context = new UserCorrectionDataContext())
            {
                var corrections = context.sp_GetForms(lemma.ToUpperInvariant(), "RU", token.Value).ToList();
                foreach (var correction in corrections.Where(form => form.Plural == plural))
                {
                    switch (correction.FormID.ToString())
                    {
                        case "И":
                            paradigm.Nominative = correction.AccentedText;
                            break;
                        case "Р":
                            paradigm.Genitive = correction.AccentedText;
                            break;
                        case "Д":
                            paradigm.Dative = correction.AccentedText;
                            break;
                        case "В":
                            paradigm.Accusative = correction.AccentedText;
                            break;
                        case "Т":
                            paradigm.Instrumental = correction.AccentedText;
                            break;
                        case "П":
                            paradigm.Prepositional = correction.AccentedText;
                            break;
                        case "М":
                            paradigm.PrepositionalWithPre = correction.AccentedText;
                            break;
                    }
                }
            }
               
        }

        public void SetUserDeclensions(UkrainianDeclensionForms paradigm, string lemma, bool plural, Guid? token)
        {
            using (UserCorrectionDataContext context = new UserCorrectionDataContext())
            {
                var corrections = context.sp_GetForms(lemma.ToUpperInvariant(), "UK", token.Value).ToList();
                    foreach (var correction in corrections.Where(form => form.Plural == plural))
                    {
                        switch (correction.FormID.ToString())
                        {
                            case "Н":
                                paradigm.Nominative = correction.AccentedText;
                                break;
                            case "Р":
                                paradigm.Genitive = correction.AccentedText;
                                break;
                            case "Д":
                                paradigm.Dative = correction.AccentedText;
                                break;
                            case "З":
                                paradigm.Accusative = correction.AccentedText;
                                break;
                            case "О":
                                paradigm.Instrumental = correction.AccentedText;
                                break;
                            case "М":
                                paradigm.Prepositional = correction.AccentedText;
                                break;
                            case "К":
                                paradigm.Vocative = correction.AccentedText;
                                break;
                        }
                    }
            }
        }
    }
}