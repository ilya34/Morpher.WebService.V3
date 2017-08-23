namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Linq;
    using Interfaces;
    using Morpher.WebService.V3.Models;

    public class UserCorrectionDatabaseService : IUserCorrection
    {
        public void SetUserDeclensions(RussianDeclensionForms paradigm, string lemma, bool plural, Guid? token)
        {
            using (UserCorrectionDataContext context = new UserCorrectionDataContext())
            {
                var corrections = context.sp_GetForms(lemma.ToUpperInvariant(), "RU", token.Value).ToList();
                foreach (var correction in corrections.Where(form => form.Plural == plural))
                {
                    string text = correction.AccentedText;
                    switch (correction.FormID.ToString())
                    {
                        case "И":
                            paradigm.Nominative = text;
                            break;
                        case "Р":
                            paradigm.Genitive = text;
                            break;
                        case "Д":
                            paradigm.Dative = text;
                            break;
                        case "В":
                            paradigm.Accusative = text;
                            break;
                        case "Т":
                            paradigm.Instrumental = text;
                            break;
                        case "П":
                            paradigm.Prepositional = text;
                            break;
                        case "М":
                            paradigm.PrepositionalWithPre = text;
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
                    string text = correction.AccentedText;
                    switch (correction.FormID.ToString())
                    {
                        case "Н":
                            paradigm.Nominative = text;
                            break;
                        case "Р":
                            paradigm.Genitive = text;
                            break;
                        case "Д":
                            paradigm.Dative = text;
                            break;
                        case "З":
                            paradigm.Accusative = text;
                            break;
                        case "О":
                            paradigm.Instrumental = text;
                            break;
                        case "М":
                            paradigm.Prepositional = text;
                            break;
                        case "К":
                            paradigm.Vocative = text;
                            break;
                    }
                }
            }
        }
    }
}