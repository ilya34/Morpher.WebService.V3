namespace Morpher.WebService.V3.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = "GetXmlUkrResult")]
    public class UkrainianDeclensionResult : UkrainianDeclensionForms
    {
        [DataMember(Name = "рід", EmitDefaultValue = false, Order = 7)]
        public string Gender { get; set; }
    }
}