using System;
using Newtonsoft.Json;

namespace Morpher.WebService.V3.Russian.Data
{
    using System.Diagnostics.CodeAnalysis;

    using General.Data;
    using System.Xml.Serialization;

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

            switch (serviceResult.Gender)
            {
                case Russian.Gender.Masculine:
                    Gender = "Мужской";
                    break;
                case Russian.Gender.Feminine:
                    Gender = "Женский";
                    break;
                case Russian.Gender.Neuter:
                    Gender = "Средний";
                    break;
                case Russian.Gender.Plural:
                case null:
                    break;
            }

            if (serviceResult.FullName != null)
            {
                FullName = new FullName(serviceResult.FullName);
            }

            Where = serviceResult.Where;
            To = serviceResult.To;
            From = serviceResult.From;
        }

        [JsonProperty(PropertyName = "род", DefaultValueHandling = DefaultValueHandling.Ignore, Order = 7)]
        [XmlElement("род", Order = 7)]
        [OnlyForPaid]
        public virtual string Gender { get; set; }

        [JsonProperty(PropertyName = "множественное", DefaultValueHandling = DefaultValueHandling.Ignore, Order = 8)]
        [XmlElement("множественное", Order = 8)]
        [CheckForPaid]
        public virtual DeclensionForms Plural { get; set; }

        [JsonProperty(PropertyName = "где", DefaultValueHandling = DefaultValueHandling.Ignore, Order = 9)]
        [XmlElement("где", Order = 9)]
        [OnlyForPaid]
        public virtual string Where { get; set; }

        [JsonProperty(Order = 10, PropertyName = "куда", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("куда", Order = 10)]
        [OnlyForPaid]
        public virtual string To { get; set; }

        [JsonProperty(Order = 11, PropertyName = "откуда", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("откуда", Order = 11)]
        [OnlyForPaid]
        public virtual string From { get; set; }

        [JsonProperty(PropertyName = "ФИО", DefaultValueHandling = DefaultValueHandling.Ignore, Order = 12)]
        [XmlElement("ФИО", Order = 12)]
        public virtual FullName FullName { get; set; }
    }
}