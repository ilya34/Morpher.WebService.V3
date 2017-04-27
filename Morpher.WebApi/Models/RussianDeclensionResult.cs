namespace Morpher.WebApi.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = "xml")]
    public class RussianDeclensionResult : RussianDeclensionForms
    {
        [DataMember(Name = "род", EmitDefaultValue = false, Order = 7)]
        public string Gender { get; set; }

        [DataMember(Name = "множественное", EmitDefaultValue = false, Order = 8)]
        public RussianDeclensionForms Plural { get; set; }

        [DataMember(Name = "ФИО", EmitDefaultValue = false, Order = 9)]
        public FullName FullName { get; set; }

        [DataMember(Order = 10, Name = "Где", EmitDefaultValue = false)]
        public string Where { get; set; }

        [DataMember(Order = 11, Name = "Куда", EmitDefaultValue = false)]
        public string To { get; set; }

        [DataMember(Order = 12, Name = "Откуда", EmitDefaultValue = false)]
        public string From { get; set; }
    }
}