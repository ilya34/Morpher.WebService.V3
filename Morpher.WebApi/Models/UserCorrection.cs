namespace Morpher.WebService.V3.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Name = "ОбъектИсправления", Namespace = "")]
    public class UserCorrectionPostModel
    {
        [DataMember(Name = "Слово")]
        public string NominateiveForm { get; set; }

        [DataMember(Name = "Исправления")]
        public List<Correction> Corrections { get; set; }
    }
}