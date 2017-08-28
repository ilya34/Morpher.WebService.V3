namespace Morpher.WebService.V3.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = "GetXmlUkrResult")]
    public class UkrainianDeclensionResult : UkrainianDeclensionForms
    {
        public UkrainianDeclensionResult()
        { 
        }

        public UkrainianDeclensionResult(UkrainianDeclensionForms forms)
            : base(forms)
        {
        }

        public UkrainianDeclensionResult(Ukrainian.DeclensionResult serviceResult)
            : base(serviceResult)
        {
            Gender = serviceResult.Gender;
        }

        [DataMember(Name = "рід", EmitDefaultValue = false, Order = 7)]
        public string Gender { get; set; }
    }
}