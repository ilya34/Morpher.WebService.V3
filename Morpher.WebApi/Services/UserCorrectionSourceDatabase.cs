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
        private readonly IMorpherCache morpherCache;

        private readonly IMorpherCache apiThrottler;

        public UserCorrectionSourceDatabase([Named("UserCorrection")] IMorpherCache morpherCache, [Named("ApiThrottler")]IMorpherCache apiThrottler)
        {
            this.morpherCache = morpherCache;
            this.apiThrottler = apiThrottler;
        }

        public virtual IList<Correction> GetUserCorrections(Guid? userId, string lemma, string language)
        {
            MorpherCacheObject cacheObject = (MorpherCacheObject)this.apiThrottler.Get(userId.ToString().ToLowerInvariant());
            

            List<UserCorrectionEntity> userCorrectionEntities =
                (List<UserCorrectionEntity>)this.morpherCache.Get(cacheObject.UserId.ToString().ToLowerInvariant());

            return userCorrectionEntities?.SingleOrDefault(entity => entity.NominativeForm == lemma.ToUpperInvariant() && entity.Language == language)?.Corrections.ToList();
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