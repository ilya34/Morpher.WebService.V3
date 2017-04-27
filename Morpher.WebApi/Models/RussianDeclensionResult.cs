namespace Morpher.WebApi.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = "xml")]
    public class RussianDeclensionResult : RussianDeclensionForms
    {
        [DataMember(Name = "род", EmitDefaultValue = false, Order = 1)]
        public string Gender { get; set; }

        [DataMember(Name = "множественное", EmitDefaultValue = false, Order = 2)]
        public RussianDeclensionForms Plural { get; set; }

        [DataMember(Name = "ФИО", EmitDefaultValue = false, Order = 3)]
        public FullName FullName { get; set; }

        [DataMember(Order = 4, Name = "Где", EmitDefaultValue = false)]
        public string Where { get; set; }

        [DataMember(Order = 5, Name = "Куда", EmitDefaultValue = false)]
        public string To { get; set; }

        [DataMember(Order = 6, Name = "Откуда", EmitDefaultValue = false)]
        public string From { get; set; }
    }
}