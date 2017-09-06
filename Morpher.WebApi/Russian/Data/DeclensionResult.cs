namespace Morpher.WebService.V3.Russian.Data
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using General.Data;

    [DataContract(Name = "xml")]
    public class DeclensionResult : DeclensionForms
    {
        public DeclensionResult()
        {
        }

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public DeclensionResult(Russian.DeclensionResult serviceResult)
            : base(serviceResult)
        {
            if (serviceResult.Plural != null)
            {
                Plural = new DeclensionForms(serviceResult.Plural);
            }

            Gender = serviceResult.Gender;

            if (serviceResult.FullName != null)
            {
                FullName = new FullName(serviceResult.FullName);
            }

            Where = serviceResult.Where;
            To = serviceResult.To;
            From = serviceResult.From;
        }

        [DataMember(Name = "род", EmitDefaultValue = false, Order = 7)]
        [OnlyForPaid]
        public virtual string Gender { get; set; }

        [DataMember(Name = "множественное", EmitDefaultValue = false, Order = 8)]
        [CheckForPayed]
        public virtual DeclensionForms Plural { get; set; }

        [DataMember(Name = "ФИО", EmitDefaultValue = false, Order = 13)]
        public virtual FullName FullName { get; set; }

        [DataMember(Order = 10, Name = "где", EmitDefaultValue = false)]
        [OnlyForPaid]
        public virtual string Where { get; set; }

        [DataMember(Order = 11, Name = "куда", EmitDefaultValue = false)]
        [OnlyForPaid]
        public virtual string To { get; set; }

        [DataMember(Order = 12, Name = "откуда", EmitDefaultValue = false)]
        [OnlyForPaid]
        public virtual string From { get; set; }
    }
}