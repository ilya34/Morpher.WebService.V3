namespace Morpher.WebService.V3.Russian.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Models;

    [DataContract]
    public class DeclensionFormsForCorrection
    {
        public DeclensionFormsForCorrection(List<Models.NameForm> nameForms)
        {
            Nominative = nameForms.SingleOrDefault(form => form.FormID == 'И')?.AccentedText;
            Genitive = nameForms.SingleOrDefault(form => form.FormID == 'Р')?.AccentedText;
            Accusative = nameForms.SingleOrDefault(form => form.FormID == 'В')?.AccentedText;
            Dative = nameForms.SingleOrDefault(form => form.FormID == 'Д')?.AccentedText;
            Instrumental = nameForms.SingleOrDefault(form => form.FormID == 'Т')?.AccentedText;
            Prepositional = nameForms.SingleOrDefault(form => form.FormID == 'П')?.AccentedText;
            Locative = nameForms.SingleOrDefault(form => form.FormID == 'М')?.AccentedText;
        }

        [DataMember(Order = 0, Name = "И", EmitDefaultValue = false)]
        public virtual string Nominative { get; set; }

        [DataMember(Order = 1, Name = "Р", EmitDefaultValue = false)]
        public virtual string Genitive { get; set; }

        [DataMember(Order = 2, Name = "Д", EmitDefaultValue = false)]
        public virtual string Dative { get; set; }

        [DataMember(Order = 3, Name = "В", EmitDefaultValue = false)]
        public virtual string Accusative { get; set; }

        [DataMember(Order = 4, Name = "Т", EmitDefaultValue = false)]
        public virtual string Instrumental { get; set; }

        [DataMember(Order = 5, Name = "П", EmitDefaultValue = false)]
        public virtual string Prepositional { get; set; }

        [DataMember(Order = 6, Name = "М", EmitDefaultValue = false)]
        public virtual string Locative { get; set; }
    }
}