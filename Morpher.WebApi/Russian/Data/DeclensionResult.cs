namespace Morpher.WebService.V3.Russian.Data
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using General.Data;
    using System.Xml.Serialization;

    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
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
        [XmlElement("род", Order = 7)]
        [OnlyForPaid]
        public virtual string Gender { get; set; }

        [DataMember(Name = "множественное", EmitDefaultValue = false, Order = 8)]
        [XmlElement("множественное", Order = 8)]
        [CheckForPayed]
        public virtual DeclensionForms Plural { get; set; }

        [DataMember(Name = "где", EmitDefaultValue = false, Order = 10)]
        [XmlElement("где", Order = 10)]
        [OnlyForPaid]
        public virtual string Where { get; set; }

        [DataMember(Order = 11, Name = "куда", EmitDefaultValue = false)]
        [XmlElement("куда", Order = 11)]
        [OnlyForPaid]
        public virtual string To { get; set; }

        [DataMember(Order = 12, Name = "откуда", EmitDefaultValue = false)]
        [XmlElement("откуда", Order = 12)]
        [OnlyForPaid]
        public virtual string From { get; set; }

        [DataMember(Name = "ФИО", EmitDefaultValue = false, Order = 13)]
        [XmlElement("ФИО", Order = 13)]
        public virtual FullName FullName { get; set; }
    }
}