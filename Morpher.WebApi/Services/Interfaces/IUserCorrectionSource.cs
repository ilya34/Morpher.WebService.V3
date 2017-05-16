namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Morpher.WebService.V3.Models;

    public interface IUserCorrectionSource
    {
        IList<NameForm> GetUserCorrections(Guid token, string lemma, string language);
    }
}
