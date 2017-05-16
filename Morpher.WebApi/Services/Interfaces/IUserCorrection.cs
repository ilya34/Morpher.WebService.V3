namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;

    using Morpher.WebService.V3.Models.Interfaces;

    public interface IUserCorrection
    {
        void SetUserDeclensions(IRussianParadigm paradigm, string lemma, bool plural, Guid token = default(Guid));

        void SetUserDeclensions(IUkrainianParadigm paradigm, string lemma, bool plural, Guid token = default(Guid));
    }
}
