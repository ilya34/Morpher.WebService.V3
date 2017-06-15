namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Generic;

    using Morpher.WebService.V3.Shared.Interfaces;
    using Morpher.WebService.V3.Shared.Models;

    public class UserCorrectionSourceFile : IUserCorrectionSource
    {
        public IList<Correction> GetUserCorrections(Guid? token, string lemma, string language)
        {
            return null;
        }
    }
}