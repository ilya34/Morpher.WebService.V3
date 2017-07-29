// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class NameForm
    {
        public Guid NameID { get; set; }

        public string FormID { get; set; }

        public bool Plural { get; set; }

        public string LanguageID { get; set; }

        public string AccentedText { get; set; }
    }
}