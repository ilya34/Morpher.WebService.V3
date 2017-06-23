namespace Morpher.WebService.V3.Shared.Models
{
    using System.Diagnostics.CodeAnalysis;
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
            foreach (var firstProp in props)
            {
                var obj = firstProp.GetValue(form);
                if (obj != null)
                {
                    firstProp.SetValue(this, obj);
                }
            }
        }

        [SuppressMessage("ReSharper", "StyleCop.SA1503")]
        public bool Equals(RussianDeclensionForms other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(this.Nominative, other.Nominative) &&
                string.Equals(this.Genitive, other.Genitive) &&
                string.Equals(this.Dative, other.Dative) &&
                string.Equals(this.Accusative, other.Accusative) &&
                string.Equals(this.Instrumental, other.Instrumental) &&
                string.Equals(this.Prepositional, other.Prepositional) &&
                string.Equals(this.PrepositionalWithPre, other.PrepositionalWithPre);
        }

        [SuppressMessage("ReSharper", "StyleCop.SA1503")]
        [SuppressMessage("ReSharper", "StyleCop.SA1126")]
        [SuppressMessage("ReSharper", "ArrangeThisQualifier")]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((RussianDeclensionForms)obj);
        }

        [SuppressMessage("ReSharper", "StyleCop.SA1119")]
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Nominative != null ? this.Nominative.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Genitive != null ? this.Genitive.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Dative != null ? this.Dative.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Accusative != null ? this.Accusative.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Instrumental != null ? this.Instrumental.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Prepositional != null ? this.Prepositional.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.PrepositionalWithPre != null ? this.PrepositionalWithPre.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}