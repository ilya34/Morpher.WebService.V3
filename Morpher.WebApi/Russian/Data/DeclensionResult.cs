namespace Morpher.WebService.V3.Russian.Data
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using General.Data;
    using System.Xml.Serialization;

    [XmlRoot("xml")]
    [DataContract]
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

        [XmlElement("род", Order = 7)]
        [OnlyForPaid]
        public virtual string Gender { get; set; }

        [XmlElement("множественное", Order = 8)]
        [CheckForPayed]
        public virtual DeclensionForms Plural { get; set; }

        [XmlElement("где", Order = 10)]
        [OnlyForPaid]
        public virtual string Where { get; set; }

        [XmlElement("куда", Order = 11)]
        [OnlyForPaid]
        public virtual string To { get; set; }

        [XmlElement("откуда", Order = 12)]
        [OnlyForPaid]
        public virtual string From { get; set; }

        [XmlElement("ФИО", Order = 13)]
        public virtual FullName FullName { get; set; }
    }
}