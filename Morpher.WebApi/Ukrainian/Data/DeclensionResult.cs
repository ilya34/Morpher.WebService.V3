namespace Morpher.WebService.V3.Ukrainian.Data
{
    using System.Xml.Serialization;
    using General.Data;

    [XmlRoot("GetXmlUkrResult")]
    public class DeclensionResult : DeclensionForms
    {
        public DeclensionResult()
        { 
        }

        public DeclensionResult(Ukrainian.DeclensionResult serviceResult)
            : base(serviceResult)
        {
            Gender = serviceResult.Gender;
        }

        [XmlElement("рід", Order = 7)]
        [OnlyForPaid]
        public string Gender { get; set; }
    }
}