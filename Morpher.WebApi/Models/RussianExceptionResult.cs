namespace Morpher.WebService.V3.Models
{
    public class RussianExceptionResult : RussianDeclensionResult
    {
        private readonly RussianEntry _entry;
        private readonly RussianDeclensionResult _declensionResult;
        private readonly RussianExceptionForms _exceptionForms;

        public RussianExceptionResult(RussianEntry entry, RussianDeclensionResult declensionResult)
        {
            _entry = entry;
            _declensionResult = declensionResult;

            if (entry.Plural != null)
            {
                _exceptionForms = new RussianExceptionForms(entry.Plural);
            }
        }

        public override RussianDeclensionForms Plural => _exceptionForms ?? _declensionResult.Plural;

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
