﻿namespace Morpher.WebService.V3.Ukrainian.Data
{
    using System.Runtime.Serialization;
    using General.Data;

    [DataContract(Name = "GetXmlUkrResult")]
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

        [DataMember(Name = "рід", EmitDefaultValue = false, Order = 7)]
        [OnlyForPaid]
        public string Gender { get; set; }
    }
}