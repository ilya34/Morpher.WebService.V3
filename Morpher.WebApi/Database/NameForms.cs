namespace Morpher.WebService.V3.Database
{
    using System;

    public class NameForms
    {
        public Guid NameID { get; set; }

        public string FormID { get; set; }

        public bool Plural { get; set; }

        public string LanguageID { get; set; }

        public string AccentedText { get; set; }
    }
}