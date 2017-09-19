using System.Xml.Serialization;

namespace Morpher.WebService.V3.Russian.Data
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using General.Data;

    [XmlRoot("множественное")]
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

        [XmlElement("И", Order = 0)]
        public virtual string Nominative { get; set; }

        [XmlElement("Р", Order = 1)]
        public virtual string Genitive { get; set; }

        [XmlElement("Д", Order = 2)]
        public virtual string Dative { get; set; }

        [XmlElement("В", Order = 3)]
        public virtual string Accusative { get; set; }

        [XmlElement("Т", Order = 4)]
        public virtual string Instrumental { get; set; }

        [XmlElement("П", Order = 5)]
        public virtual string Prepositional { get; set; }

        [XmlElement("П_о", Order = 8)]
        [OnlyForPaid]
        public virtual string PrepositionalWithPre { get; set; }
    }
}