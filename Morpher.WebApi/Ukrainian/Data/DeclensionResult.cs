namespace Morpher.WebService.V3.Ukrainian.Data
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using General;

    [DataContract(Name = "GetXmlUkrResult")]
    [XmlRoot("GetXmlUkrResult")]
    public class DeclensionResult : DeclensionForms
    {
        public DeclensionResult()
        { 
        }

        public DeclensionResult(Ukrainian.DeclensionResult serviceResult)
            : base(serviceResult)
        {
            switch (serviceResult.Gender)
            {
                case Ukrainian.Gender.Masculine:
                    Gender = "Чоловічий";
                    break;
                case Ukrainian.Gender.Feminine:
                    Gender = "Жіночий";
                    break;
                case Ukrainian.Gender.Neuter:
                    Gender = "Середній";
                    break;
                case Ukrainian.Gender.Plural:
                case null:
                    break;
            }
        }

        [DataMember(Name = "рід", EmitDefaultValue = false, Order = 7)]
        [XmlElement("рід", Order = 7)]
        [OnlyForPaid]
        public string Gender { get; set; }
    }
}