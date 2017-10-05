namespace Morpher.WebService.V3.General.Data
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [XmlType("gender")]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Gender
    {
        [XmlEnum("masculine")]
        Masculine,
        [XmlEnum("feminine")]
        Feminine,
        [XmlEnum("neuter")]
        Neuter,
        [XmlEnum("plural")]
        Plural
    }
}