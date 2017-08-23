namespace Morpher.WebService.V3.Models
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType("single-number-forms")]
    public class RussianDeclensionForms
    {
        [DataMember(Order = 0, Name = "И", EmitDefaultValue = false)]
        [XmlElement("И")]
        public string Nominative { get; set; }

        [DataMember(Order = 1, Name = "Р", EmitDefaultValue = false)]
        [XmlElement("Р")]
        public string Genitive { get; set; }

        [DataMember(Order = 2, Name = "Д", EmitDefaultValue = false)]
        [XmlElement("Д")]
        public string Dative { get; set; }

        [DataMember(Order = 3, Name = "В", EmitDefaultValue = false)]
        [XmlElement("В")]
        public string Accusative { get; set; }

        [DataMember(Order = 4, Name = "Т", EmitDefaultValue = false)]
        [XmlElement("Т")]
        public string Instrumental { get; set; }

        [DataMember(Order = 5, Name = "П", EmitDefaultValue = false)]
        [XmlElement("П")]
        public string Prepositional { get; set; }

        [DataMember(Order = 6, Name = "П_о", EmitDefaultValue = false)]
        [XmlElement("М")]
        public string PrepositionalWithPre { get; set; }

        public void AddOrUpdate(RussianDeclensionForms form)
        {
            var props = typeof(RussianDeclensionForms).GetProperties();
            foreach (var prop in props)
            {
                object value = prop.GetValue(form);
                if (value != null)
                {
                    prop.SetValue(this, value);
                }
            }
        }
    }
}