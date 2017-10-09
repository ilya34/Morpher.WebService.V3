namespace Morpher.WebService.V3.Russian.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class LookupEntry
    {
        public LookupEntry(List<NameForm> forms)
        {
            Singular = new DeclensionResult(forms.Where(form => !form.Plural).ToList());
            Plural = new DeclensionResult(forms.Where(form => form.Plural).ToList());
        }

        public DeclensionResult Singular { get; set; }

        public DeclensionResult Plural { get; set; }

        public Gender Gender { get; set; }
    }
}