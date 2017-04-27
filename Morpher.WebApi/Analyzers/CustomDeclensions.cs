namespace Morpher.WebApi.Analyzers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Dapper;

    using Morpher.WebApi.Analyzers.Interfaces;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Interfaces;

    public class CustomDeclensions : ICustomDeclensions
    {
        private readonly string connectionString;

        public CustomDeclensions(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void SetUserDeclensions(IRussianParadigm paradigm, string lemma, bool plural, Guid token = new Guid())
        {
            var nameForms = this.GetUserDeclensions(token, lemma, "RU");
            if (nameForms != null)
            {
                foreach (var nameForm in nameForms.Where(form => form.Plural == plural))
                {
                    switch (nameForm.FormID)
                    {
                        case "И":
                            paradigm.Nominative = nameForm.AccentedText;
                            break;
                        case "Р":
                            paradigm.Genitive = nameForm.AccentedText;
                            break;
                        case "Д":
                            paradigm.Dative = nameForm.AccentedText;
                            break;
                        case "В":
                            paradigm.Accusative = nameForm.AccentedText;
                            break;
                        case "Т":
                            paradigm.Instrumental = nameForm.AccentedText;
                            break;
                        case "П":
                            paradigm.Prepositional = nameForm.AccentedText;
                            break;
                        case "М":
                            paradigm.Locative = nameForm.AccentedText;
                            break;
                    }
                }
            }
        }

        public void SetUserDeclensions(IUkrainianParadigm paradigm, string lemma, bool plural, Guid token = new Guid())
        {
            var nameForms = this.GetUserDeclensions(token, lemma, "UK");
            if (nameForms != null)
            {
                foreach (var nameForm in nameForms.Where(form => form.Plural == plural))
                {
                    switch (nameForm.FormID)
                    {
                        case "Н":
                            paradigm.Nominative = nameForm.AccentedText;
                            break;
                        case "Р":
                            paradigm.Genitive = nameForm.AccentedText;
                            break;
                        case "Д":
                            paradigm.Dative = nameForm.AccentedText;
                            break;
                        case "З":
                            paradigm.Accusative = nameForm.AccentedText;
                            break;
                        case "О":
                            paradigm.Instrumental = nameForm.AccentedText;
                            break;
                        case "М":
                            paradigm.Prepositional = nameForm.AccentedText;
                            break;
                        case "К":
                            paradigm.Vocative = nameForm.AccentedText;
                            break;
                    }
                }
            }
        }

        private IList<NameForm> GetUserDeclensions(Guid token, string lemma, string language)
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