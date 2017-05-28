namespace Morpher.WebService.V3.Shared.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Morpher.WebService.V3.Shared.Models;

    public interface IUserCorrectionSource
    {
        IList<NameForm> GetUserCorrections(Guid token, string lemma, string language);
    }
}
