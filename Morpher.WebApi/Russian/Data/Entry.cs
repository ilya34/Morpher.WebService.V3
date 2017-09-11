namespace Morpher.WebService.V3.Russian.Data
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Entry
    {
        public Entry(DeclensionFormsForCorrection  singular, DeclensionFormsForCorrection plural)
        {
            Singular = singular;
            Plural = plural;
        }

        public Entry()
        {
        }

        [DataMember(Name = "singular", EmitDefaultValue = false)]
        public DeclensionFormsForCorrection Singular { get; set; }

        [DataMember(Name = "plural", EmitDefaultValue = false)]
        public DeclensionFormsForCorrection Plural { get; set; }
    }
}
