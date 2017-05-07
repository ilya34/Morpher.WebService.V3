namespace Morpher.WebApi.Services.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Morpher.WebApi.Models;

    public interface IUserCorrectionSource
    {
        IList<NameForm> GetUserCorrections(Guid token, string lemma, string language);
    }
}
