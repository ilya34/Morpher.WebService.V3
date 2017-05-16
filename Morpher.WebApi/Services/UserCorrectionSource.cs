namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Dapper;

    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;

    public class UserCorrectionSource : IUserCorrectionSource
    {
        private readonly string connectionString;

        public UserCorrectionSource(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public virtual IList<NameForm> GetUserCorrections(Guid token, string lemma, string language)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                IEnumerable<NameForm> forms = connection.Query<NameForm>(
                    "sp_GetForms",
                    new { normalizedLemma = lemma, language, token },
                    commandType: CommandType.StoredProcedure);

                if (forms == null)
                {
                    return null;
                }

                return forms as IList<NameForm> ?? forms.ToList();
            }

        }
    }
}