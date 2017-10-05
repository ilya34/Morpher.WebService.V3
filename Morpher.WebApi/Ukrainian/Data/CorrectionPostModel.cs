namespace Morpher.WebService.V3.Ukrainian.Data
{
    using System.Collections.Generic;
    using General;
    using Models;

    public class CorrectionPostModel
    {
        public string Н { get; set; }
        public string Р { get; set; }
        public string Д { get; set; }
        public string З { get; set; }
        public string О { get; set; }
        public string М { get; set; }
        public string К { get; set; }
        public string М_Н { get; set; }
        public string М_Р { get; set; }
        public string М_Д { get; set; }
        public string М_З { get; set; }
        public string М_О { get; set; }
        public string М_М { get; set; }
        public string М_К { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(Н)
                   && string.IsNullOrWhiteSpace(Р)
                   && string.IsNullOrWhiteSpace(Д)
                   && string.IsNullOrWhiteSpace(З)
                   && string.IsNullOrWhiteSpace(О)
                   && string.IsNullOrWhiteSpace(М)
                   && string.IsNullOrWhiteSpace(К)
                   && string.IsNullOrWhiteSpace(М_Н)
                   && string.IsNullOrWhiteSpace(М_Р)
                   && string.IsNullOrWhiteSpace(М_Д)
                   && string.IsNullOrWhiteSpace(М_З)
                   && string.IsNullOrWhiteSpace(М_О)
                   && string.IsNullOrWhiteSpace(М_М)
                   && string.IsNullOrWhiteSpace(М_К);
        }

        public static implicit operator List<NameForm>(CorrectionPostModel model)
        {
            List<NameForm> nameForms = new List<NameForm>();

            if (!string.IsNullOrWhiteSpace(model.Н))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.Н,
                    FormID = 'Н',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            if (!string.IsNullOrWhiteSpace(model.Р))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.Р,
                    FormID = 'Р',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            if (!string.IsNullOrWhiteSpace(model.Д))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.Д,
                    FormID = 'Д',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            if (!string.IsNullOrWhiteSpace(model.З))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.З,
                    FormID = 'З',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            if (!string.IsNullOrWhiteSpace(model.О))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.О,
                    FormID = 'О',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            if (!string.IsNullOrWhiteSpace(model.М))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.М,
                    FormID = 'М',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            if (!string.IsNullOrWhiteSpace(model.К))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.К,
                    FormID = 'К',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            // Plural
            if (!string.IsNullOrWhiteSpace(model.М_Н))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_Н,
                    FormID = 'Н',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            if (!string.IsNullOrWhiteSpace(model.М_Р))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_Р,
                    FormID = 'Р',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            if (!string.IsNullOrWhiteSpace(model.М_Д))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_Д,
                    FormID = 'Д',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            if (!string.IsNullOrWhiteSpace(model.М_З))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_З,
                    FormID = 'З',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            if (!string.IsNullOrWhiteSpace(model.М_О))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_О,
                    FormID = 'О',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            if (!string.IsNullOrWhiteSpace(model.М_М))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_М,
                    FormID = 'М',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            if (!string.IsNullOrWhiteSpace(model.М_К))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_К,
                    FormID = 'К',
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }

            return nameForms;
        }
    }
}