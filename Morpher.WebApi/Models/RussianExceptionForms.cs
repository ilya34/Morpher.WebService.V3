namespace Morpher.WebService.V3.Models
{
    public class RussianExceptionForms : RussianDeclensionForms
    {
        private readonly RussianDeclensionForms _forms;

        public RussianExceptionForms(RussianDeclensionForms forms)
        {
            _forms = forms;
        }

        public override string Nominative
            => _forms.Nominative ?? base.Nominative;

        public override string Genitive
            => _forms.Genitive ?? base.Genitive;

        public override string Dative
            => _forms.Dative ?? base.Dative;

        public override string Accusative
            => _forms.Accusative ?? base.Accusative;

        public override string Instrumental
            => _forms.Instrumental ?? base.Instrumental;

        public override string Prepositional
            => _forms.Prepositional ?? base.Prepositional;

        public override string PrepositionalWithPre
            => _forms.PrepositionalWithPre ?? base.PrepositionalWithPre;
    }
}