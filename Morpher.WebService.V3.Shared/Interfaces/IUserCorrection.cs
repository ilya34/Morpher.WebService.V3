// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Interfaces
{
    using System;

    using Morpher.WebService.V3.Shared.Models;

    public interface IUserCorrection
    {
        void SetUserDeclensions(RussianDeclensionForms paradigm, string lemma, bool plural, Guid? token);

        void SetUserDeclensions(UkrainianDeclensionForms paradigm, string lemma, bool plural, Guid? token);
    }
}
