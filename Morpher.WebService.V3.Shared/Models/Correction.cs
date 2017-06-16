namespace Morpher.WebService.V3.Shared.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract(Name = "Исправление", Namespace = "")]
    [SuppressMessage("ReSharper", "StyleCop.SA1503")]
    public class Correction
    {
        public static IEqualityComparer<Correction> FormPluralComparer { get; } = new FormPluralEqualityComparer();

        [DataMember(Name = "Лемма")]
        public string Lemma { get; set; }

        [DataMember(Name = "Падеж")]
        public string Form { get; set; }

        [DataMember(Name = "Множественное")]
        public bool Plural { get; set; }

        private sealed class FormPluralEqualityComparer : IEqualityComparer<Correction>
        {
            public bool Equals(Correction x, Correction y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.Form, y.Form) && x.Plural == y.Plural;
            }

            public int GetHashCode(Correction obj)
            {
                unchecked
                {
                    return ((obj.Form != null ? obj.Form.GetHashCode() : 0) * 397) ^ obj.Plural.GetHashCode();
                }
            }
        }
    }
}