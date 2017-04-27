namespace Morpher.WebApi.Services.Interfaces
{
    using System;

    using Morpher.WebApi.Models.Interfaces;

    public interface ICustomDeclensions
    {
        void SetUserDeclensions(IRussianParadigm paradigm, string lemma, bool plural, Guid token = default(Guid));

        void SetUserDeclensions(IUkrainianParadigm paradigm, string lemma, bool plural, Guid token = default(Guid));
    }
}
