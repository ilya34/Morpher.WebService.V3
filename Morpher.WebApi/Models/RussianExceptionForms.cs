namespace Morpher.WebService.V3.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class RussianExceptionForms : RussianDeclensionForms
    {
        private readonly RussianDeclensionForms _correction;
        private readonly RussianDeclensionForms _inner;

        public RussianExceptionForms(RussianDeclensionForms correction, RussianDeclensionForms inner)
        {
            _correction = correction;
            _inner = inner;
        }

        public override string Nominative
            => _correction.Nominative ?? _inner.Nominative;

        public override string Genitive
            => _correction.Genitive ?? _inner.Genitive;

        public override string Dative
            => _correction.Dative ?? _inner.Dative;

        public override string Accusative
            => _correction.Accusative ?? _inner.Accusative;

        public override string Instrumental
            => _correction.Instrumental ?? _inner.Instrumental;

        public override string Prepositional
            => _correction.Prepositional ?? _inner.Prepositional;

        public override string PrepositionalWithPre
            => _correction.PrepositionalWithPre ?? _inner.PrepositionalWithPre;
    }
}