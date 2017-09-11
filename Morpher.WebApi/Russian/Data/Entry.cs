namespace Morpher.WebService.V3.Russian.Data
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Entry
    {
        public Entry(DeclensionForms singular, DeclensionForms plural)
        {
            Singular = singular;
            Plural = plural;
        }

        public Entry()
        {
        }

        [DataMember(Name = "singular", EmitDefaultValue = false)]
        public DeclensionForms Singular { get; set; }

        [DataMember(Name = "plural", EmitDefaultValue = false)]
        public DeclensionForms Plural { get; set; }
    }
}
