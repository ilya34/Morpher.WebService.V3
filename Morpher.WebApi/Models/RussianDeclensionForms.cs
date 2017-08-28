namespace Morpher.WebService.V3.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Helpers;

    [DataContract]
    [XmlType("single-number-forms")]
    public class RussianDeclensionForms
    {
        public RussianDeclensionForms()
        {
        }

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public RussianDeclensionForms(List<NameForm> nameForms)
        {
            Nominative = nameForms.SingleOrDefault(form => form.FormID == 'И')?.AccentedText;
            Genitive = nameForms.SingleOrDefault(form => form.FormID == 'И')?.AccentedText;
            Accusative = nameForms.SingleOrDefault(form => form.FormID == 'И')?.AccentedText;
            Dative = nameForms.SingleOrDefault(form => form.FormID == 'И')?.AccentedText;
            Instrumental = nameForms.SingleOrDefault(form => form.FormID == 'И')?.AccentedText;
            Prepositional = nameForms.SingleOrDefault(form => form.FormID == 'И')?.AccentedText;
            PrepositionalWithPre = nameForms.SingleOrDefault(form => form.FormID == 'И')?.AccentedText;
        }

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public RussianDeclensionForms(Russian.DeclensionForms serviceResult)
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
        [OnlyForPayed]
        public virtual string PrepositionalWithPre { get; set; }

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