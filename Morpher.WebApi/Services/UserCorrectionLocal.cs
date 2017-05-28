namespace Morpher.WebService.V3.Services
{
    using System;

    using Morpher.WebSerivce.V3.Shared.Models;
    using Morpher.WebService.V3.Services.Interfaces;

    public class UserCorrectionLocal : IUserCorrection
    {
        public void SetUserDeclensions(RussianDeclensionForms paradigm, string lemma, bool plural, Guid token = new Guid())
        {
            return;
        }

        public void SetUserDeclensions(UkrainianDeclensionForms paradigm, string lemma, bool plural, Guid token = new Guid())
        {
            return;
        }
    }
}