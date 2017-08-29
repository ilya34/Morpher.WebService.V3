namespace Morpher.WebService.V3.Russian.Data
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ExceptionResult : DeclensionResult
    {
        private readonly Entry _entry;
        private readonly DeclensionResult _declensionResult;
        private readonly ExceptionForms _exceptionForms;

        public ExceptionResult(Entry entry, DeclensionResult declensionResult)
        {
            _entry = entry;
            _declensionResult = declensionResult;

            if (entry.Plural != null)
            {
                _exceptionForms = new ExceptionForms(entry.Plural, declensionResult.Plural);
            }
        }

        public override DeclensionForms Plural => _exceptionForms ?? _declensionResult.Plural;

        public override string Nominative 
            => _entry.Singular.Nominative ?? _declensionResult.Nominative;

        public override string Genitive
            => _entry.Singular.Genitive ?? _declensionResult.Genitive;

        public override string Dative
            => _entry.Singular.Dative ?? _declensionResult.Dative;

        public override string Accusative
            => _entry.Singular.Accusative ?? _declensionResult.Accusative;

        public override string Instrumental
            => _entry.Singular.Instrumental ?? _declensionResult.Instrumental;

        public override string Prepositional
            => _entry.Singular.Prepositional ?? _declensionResult.Prepositional;

        public override string PrepositionalWithPre
            => _entry.Singular.PrepositionalWithPre ?? _declensionResult.PrepositionalWithPre;
    }
}
