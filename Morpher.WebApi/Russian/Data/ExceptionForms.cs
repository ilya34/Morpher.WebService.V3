namespace Morpher.WebService.V3.Russian.Data
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ExceptionForms : DeclensionForms
    {
        private readonly DeclensionForms _correction;
        private readonly DeclensionForms _inner;

        public ExceptionForms(DeclensionForms correction, DeclensionForms inner)
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