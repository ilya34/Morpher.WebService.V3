// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = "GetXmlUkrResult")]
    public class UkrainianDeclensionResult : UkrainianDeclensionForms
    {
        [DataMember(Name = "рід", EmitDefaultValue = false, Order = 7)]
        public string Gender { get; set; }
    }
}