namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;

    using Morpher.WebSerivce.V3.Shared.Interfaces;

    public interface IUserCorrection
    {
        void SetUserDeclensions(IRussianParadigm paradigm, string lemma, bool plural, Guid token = default(Guid));

        void SetUserDeclensions(IUkrainianParadigm paradigm, string lemma, bool plural, Guid token = default(Guid));
    }
}
