namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Microsoft.Ajax.Utilities;

    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Shared.Interfaces;
    using Morpher.WebService.V3.Shared.Models;
    using Morpher.WebService.V3.Shared.Models.Exceptions;

    public class UserCorrectionService : IUserCorrection
    {
        private static readonly List<string> RussianAllowedForms = new List<string>() { "В", "Д", "И", "М", "П", "Р", "Т" };

        private static readonly List<string> UkrainianAllowedForms = new List<string>() { "Д", "З", "К", "М", "Н", "О", "Р" };

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

        public void NewCorrection(UserCorrectionEntity entity, Guid? token)
        {
            switch (entity.Language)
            {
                case "RU": this.NewCorrectionRu(entity, token);
                    break;
                case "UK": this.NewCorrectionUk(entity, token);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(entity.Language));
            }
        }

        public bool RemoveCorrection(string lemma, string language, Guid? token)
        {
            return this.correctionSource.RemoveCorrection(token, language, lemma.ToUpperInvariant());
        }

        private void NewCorrectionRu(UserCorrectionEntity entity, Guid? token)
        {
            this.ValidateModel(entity, RussianAllowedForms);
            this.correctionSource.AssignNewCorrection(token, entity);
        }

        private void NewCorrectionUk(UserCorrectionEntity entity, Guid? token)
        {
            this.ValidateModel(entity, UkrainianAllowedForms);
            this.correctionSource.AssignNewCorrection(token, entity);
        }

        private void ValidateModel(UserCorrectionEntity entity, List<string> allowedForms)
        {
            if (string.IsNullOrWhiteSpace(entity?.NominativeForm))
            {
                throw new ModelNotValid("Лемма не должна быть null или пустой");
            }

            // падежи должны быть уникальными
            bool isSinglularFormsUnique =
                entity.Corrections.Where(correction => correction.Plural == false)
                    .DistinctBy(correction => correction.Lemma).Count()
                == entity.Corrections.Count(correction => correction.Plural == false);

            bool isPluralFormsUnique =
                entity.Corrections.Where(correction => correction.Plural == true)
                    .DistinctBy(correction => correction.Lemma).Count()
                == entity.Corrections.Count(correction => correction.Plural == true);

            if (!(isSinglularFormsUnique && isPluralFormsUnique))
            {
                throw new ModelNotValid("Падежи не должны повторяться");
            }

            bool isAllFormsCorrect =
                entity
                    .Corrections
                    .Aggregate(true, (current, correction) => current & allowedForms.Contains(correction.Form.ToUpperInvariant()));

            if (!isAllFormsCorrect)
            {
                throw new ModelNotValid("Неизвестный падеж");
            }

            if (entity.Corrections.Any(correction => string.IsNullOrWhiteSpace(correction.Lemma)))
            {
                throw new ModelNotValid("Массив исправлений соддержит пустую строку в поле Лемма");
            }
        }
    }
}