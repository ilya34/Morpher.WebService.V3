namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Interfaces;
    using Morpher.WebService.V3.Shared.Models;

    using Ninject;

    public class UserCorrectionSourceDatabase : IUserCorrectionSource
    {
        public UserCorrectionSourceDatabase()
        {
        }

        public virtual IList<Correction> GetUserCorrections(Guid? userId, string lemma, string language)
        {
            using (UserCorrectionDataContext dataContext = new UserCorrectionDataContext())
            {
                var result = dataContext.sp_GetForms(lemma, language, userId.Value);

                return result.Select(formsResult => new Correction()
                {
                    Lemma = formsResult.AccentedText,
                    Plural = formsResult.Plural,
                    Form = formsResult.FormID.ToString()
                }).ToList();
            }
        }

        public void AssignNewCorrection(Guid? token, UserCorrectionEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCorrection(Guid? token, string language, string lemma)
        {
            throw new NotImplementedException();
        }

    }
}