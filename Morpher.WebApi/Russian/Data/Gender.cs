namespace Morpher.WebService.V3.Russian.Data
{
    using System.Xml.Serialization;

    [XmlType("gender")]
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