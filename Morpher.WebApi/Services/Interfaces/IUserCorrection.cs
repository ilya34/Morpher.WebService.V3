namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;

    using Morpher.WebService.V3.Shared.Models;

    public interface IUserCorrection
    {
        void SetUserDeclensions(RussianDeclensionForms paradigm, string lemma, bool plural, Guid token = default(Guid));

        void SetUserDeclensions(UkrainianDeclensionForms paradigm, string lemma, bool plural, Guid token = default(Guid));
    }
}
