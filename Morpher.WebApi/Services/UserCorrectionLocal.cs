namespace Morpher.WebService.V3.Services
{
    using System;

    using Morpher.WebSerivce.V3.Shared.Interfaces;
    using Morpher.WebService.V3.Services.Interfaces;

    public class UserCorrectionLocal : IUserCorrection
    {
        public void SetUserDeclensions(IRussianParadigm paradigm, string lemma, bool plural, Guid token = new Guid())
        {
            return;
        }

        public void SetUserDeclensions(IUkrainianParadigm paradigm, string lemma, bool plural, Guid token = new Guid())
        {
            return;
        }
    }
}