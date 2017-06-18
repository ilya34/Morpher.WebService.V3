namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;

    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Interfaces;
    using Morpher.WebService.V3.Shared.Models;

    using Newtonsoft.Json;

    using Ninject;

    public class UserCorrectionSourceFile : IUserCorrectionSource
    {
        public static readonly string AppDataFolder = HttpContext.Current.Server.MapPath("~/App_Data/");

        private readonly IMorpherCache morpherCache;

        public UserCorrectionSourceFile([Named("UserCorrection")] IMorpherCache morpherCache)
        {
            this.morpherCache = morpherCache;
        }

        public IList<Correction> GetUserCorrections(Guid? token, string lemma, string language)
        {
            List<UserCorrectionEntity> userCorrectionEntities =
                (List<UserCorrectionEntity>)this.morpherCache.Get("local");

            return userCorrectionEntities?.SingleOrDefault(entity => entity.NominativeForm == lemma.ToUpperInvariant() && entity.Language == language)?.Corrections.ToList();
        }

        public void AssignNewCorrection(Guid? token, UserCorrectionEntity entity)
        {
            List<UserCorrectionEntity> entities = (List<UserCorrectionEntity>)this.morpherCache.Get("local");

            UserCorrectionEntity cacheEntity =
                entities.SingleOrDefault(correctionEntity => correctionEntity.NominativeForm == entity.NominativeForm && correctionEntity.Language == entity.Language);

            if (cacheEntity != null)
            {
                cacheEntity.AddOrUpdate(entity.Corrections);
            }
            else
            {
                entities.Add(entity);
            }

            string json = JsonConvert.SerializeObject(entities, Formatting.Indented);
            File.WriteAllText($"{AppDataFolder}/UserDict.json", json, Encoding.UTF8);
        }

        public bool RemoveCorrection(Guid? token, string language, string lemma)
        {
            List<UserCorrectionEntity> entities = (List<UserCorrectionEntity>)this.morpherCache.Get("local");

            UserCorrectionEntity entity =
                entities.SingleOrDefault(correctionEntity => correctionEntity.NominativeForm == lemma && correctionEntity.Language == language);

            if (entity != null)
            {
                entities.Remove(entity);
                string json = JsonConvert.SerializeObject(entities, Formatting.Indented);
                File.WriteAllText($"{AppDataFolder}/UserDict.json", json, Encoding.UTF8);
                return true;
            }

            return false;
        }
    }
}