namespace Morpher.WebService.V3.Russian.Data
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using General.Data;

    [DataContract(Name = "множественное")]
    [XmlType("single-number-forms")]
    public class DeclensionForms
    {
        /// <summary>
        /// Используется в LibAnalyzer
        /// </summary>
        public DeclensionForms()
        {
        }

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public DeclensionForms(List<Models.NameForm> nameForms)
        {
            Nominative = nameForms.SingleOrDefault(form => form.FormID == 'И')?.AccentedText;
            Genitive = nameForms.SingleOrDefault(form => form.FormID == 'Р')?.AccentedText;
            Accusative = nameForms.SingleOrDefault(form => form.FormID == 'В')?.AccentedText;
            Dative = nameForms.SingleOrDefault(form => form.FormID == 'Д')?.AccentedText;
            Instrumental = nameForms.SingleOrDefault(form => form.FormID == 'Т')?.AccentedText;
            Prepositional = nameForms.SingleOrDefault(form => form.FormID == 'П')?.AccentedText;
            PrepositionalWithPre = nameForms.SingleOrDefault(form => form.FormID == 'М')?.AccentedText;
        }

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public DeclensionForms(Russian.DeclensionForms serviceResult)
        {
            Nominative = serviceResult.Nominative;
            Genitive = serviceResult.Genitive;
            Dative = serviceResult.Dative;
            Accusative = serviceResult.Accusative;
            Instrumental = serviceResult.Instrumental;
            Prepositional = serviceResult.Prepositional;
            PrepositionalWithPre = serviceResult.PrepositionalWithO;
        }

        [DataMember(Order = 0, Name = "И", EmitDefaultValue = false)]
        [XmlElement("И")]
        public virtual string Nominative { get; set; }

        [DataMember(Order = 1, Name = "Р", EmitDefaultValue = false)]
        [XmlElement("Р")]
        public virtual string Genitive { get; set; }

        [DataMember(Order = 2, Name = "Д", EmitDefaultValue = false)]
        [XmlElement("Д")]
        public virtual string Dative { get; set; }

        [DataMember(Order = 3, Name = "В", EmitDefaultValue = false)]
        [XmlElement("В")]
        public virtual string Accusative { get; set; }

        [DataMember(Order = 4, Name = "Т", EmitDefaultValue = false)]
        [XmlElement("Т")]
        public virtual string Instrumental { get; set; }

        [DataMember(Order = 5, Name = "П", EmitDefaultValue = false)]
        [XmlElement("П")]
        public virtual string Prepositional { get; set; }

        [DataMember(Order = 6, Name = "П_о", EmitDefaultValue = false)]
        [XmlElement("М")]
        [OnlyForPaid]
        public virtual string PrepositionalWithPre { get; set; }
    }
}