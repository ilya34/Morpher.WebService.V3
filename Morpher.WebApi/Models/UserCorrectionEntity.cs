﻿namespace Morpher.WebService.V3.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract(Name = "Объект", Namespace = "")]
    public class UserCorrectionEntity
    {
        [DataMember(Name = "Лемма")]
        public string NominativeForm { get; set; }

        [DataMember(Name = "Язык")]
        public string Language { get; set; }

        [DataMember(Name = "Исправления")]
        public List<Correction> Corrections { get; set; }

        public void AddOrUpdate(List<Correction> corrections)
        {
            var diff = corrections.Except(this.Corrections, Correction.FormPluralComparer);
            var intersect = this.Corrections.Intersect(corrections, Correction.FormPluralComparer);
            foreach (var i in intersect)
            {
                i.Lemma = corrections.First(correction => correction.Form == i.Form && correction.Plural == i.Plural).Lemma;
            }

            this.Corrections.AddRange(diff);
        }
    }
}