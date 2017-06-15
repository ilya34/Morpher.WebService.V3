namespace Morpher.WebService.V3.Shared.Models
{
    using System.Collections.Generic;

    public class UserCorrectionEntity
    {
        public string NominativeForm { get; set; }

        public string Language { get; set; }

        public List<Correction> Corrections { get; set; }
    }
}