namespace Morpher.WebApi.Services
{
    using System;

    using Morpher.WebApi.Models.Interfaces;
    using Morpher.WebApi.Services.Interfaces;

    public class CustomDeclensionsLocal : ICustomDeclensions
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