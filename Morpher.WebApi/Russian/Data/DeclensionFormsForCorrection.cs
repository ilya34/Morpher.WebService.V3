namespace Morpher.WebService.V3.Russian.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    [XmlRoot]
    public class DeclensionFormsForCorrection
    {
        public DeclensionFormsForCorrection()
        {
        }

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

        [XmlElement("И")]
        public virtual string Nominative { get; set; }

        [XmlElement("Р")]
        public virtual string Genitive { get; set; }

        [XmlElement("Д")]
        public virtual string Dative { get; set; }

        [XmlElement("В")]
        public virtual string Accusative { get; set; }

        [XmlElement("Т")]
        public virtual string Instrumental { get; set; }

        [XmlElement("П")]
        public virtual string Prepositional { get; set; }

        [XmlElement("М")]
        public virtual string Locative { get; set; }
    }
}