namespace Morpher.WebApi.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = "GetXmlUkrResult")]
    public class UkrainianDeclensionResult : UkrainianDeclensionForms
    {
        [DataMember(Name = "рід", EmitDefaultValue = false)]
        public string Gender { get; set; }
    }
}