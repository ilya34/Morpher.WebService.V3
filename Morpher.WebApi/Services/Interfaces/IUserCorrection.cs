namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;
    using Models;

    public interface IUserCorrection
    {
        void SetUserDeclensions(RussianDeclensionForms paradigm, string lemma, bool plural, Guid? token);

        void SetUserDeclensions(UkrainianDeclensionForms paradigm, string lemma, bool plural, Guid? token);
    }
}
