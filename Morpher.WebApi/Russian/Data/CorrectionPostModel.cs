namespace Morpher.WebService.V3.Russian.Data
{
    using System;
    using System.Collections.Generic;
    using Models;

    /// <summary>
    /// Microsoft: "At most one parameter is allowed to read from the message body."
    /// Этот класс нужен что бы получать пользовательские исправление как x-form-www-urlencoded
    /// </summary>
    /// <remarks>Да я знаю про IModelBinder, но Сергей С. опять будет говорить про reflection и что можно проще)</remarks>
    public class CorrectionPostModel
    {
        public string И { get; set; }
        public string Р { get; set; }
        public string Д { get; set; }
        public string В { get; set; }
        public string Т { get; set; }
        public string П { get; set; }
        public string П_о { get; set; }
        public string М_И { get; set; }
        public string М_Р { get; set; }
        public string М_Д { get; set; }
        public string М_В { get; set; }
        public string М_Т { get; set; }
        public string М_П { get; set; }
        public string М_П_о { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(И)
                   && string.IsNullOrWhiteSpace(Р)
                   && string.IsNullOrWhiteSpace(Д)
                   && string.IsNullOrWhiteSpace(В)
                   && string.IsNullOrWhiteSpace(Т)
                   && string.IsNullOrWhiteSpace(П)
                   && string.IsNullOrWhiteSpace(П_о)
                   && string.IsNullOrWhiteSpace(М_И)
                   && string.IsNullOrWhiteSpace(М_Р)
                   && string.IsNullOrWhiteSpace(М_Д)
                   && string.IsNullOrWhiteSpace(М_В)
                   && string.IsNullOrWhiteSpace(М_Т)
                   && string.IsNullOrWhiteSpace(М_П)
                   && string.IsNullOrWhiteSpace(М_П_о);
        }

        public static implicit operator List<NameForm>(CorrectionPostModel model)
        {
            List<NameForm> nameForms = new List<NameForm>();

            if (!string.IsNullOrWhiteSpace(model.И))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.И,
                    FormID = 'И',
                    LanguageID = "RU"
                });
            }

            if (!string.IsNullOrWhiteSpace(model.Р))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.Р,
                    FormID = 'Р',
                    LanguageID = "RU"
                });
            }

            if (!string.IsNullOrWhiteSpace(model.Д))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.Д,
                    FormID = 'Д',
                    LanguageID = "RU"
                });
            }

            if (!string.IsNullOrWhiteSpace(model.В))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.В,
                    FormID = 'В',
                    LanguageID = "RU"
                });
            }

            if (!string.IsNullOrWhiteSpace(model.Т))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.Т,
                    FormID = 'Т',
                    LanguageID = "RU"
                });
            }

            if (!string.IsNullOrWhiteSpace(model.П))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.П,
                    FormID = 'П',
                    LanguageID = "RU"
                });
            }

            if (!string.IsNullOrWhiteSpace(model.П_о))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = false,
                    AccentedText = model.П_о,
                    FormID = 'М',
                    LanguageID = "RU"
                });
            }

            // Plural
            if (!string.IsNullOrWhiteSpace(model.М_И))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_И,
                    FormID = 'И',
                    LanguageID = "RU"
                });
            }

            if (!string.IsNullOrWhiteSpace(model.М_Р))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_Р,
                    FormID = 'Р',
                    LanguageID = "RU"
                });
            }

            if (!string.IsNullOrWhiteSpace(model.М_Д))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_Д,
                    FormID = 'Д',
                    LanguageID = "RU"
                });
            }

            if (!string.IsNullOrWhiteSpace(model.М_В))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_В,
                    FormID = 'В',
                    LanguageID = "RU"
                });
            }

            if (!string.IsNullOrWhiteSpace(model.М_Т))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_Т,
                    FormID = 'Т',
                    LanguageID = "RU"
                });
            }

            if (!string.IsNullOrWhiteSpace(model.М_П))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_П,
                    FormID = 'П',
                    LanguageID = "RU"
                });
            }

            if (!string.IsNullOrWhiteSpace(model.М_П_о))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = true,
                    AccentedText = model.М_П_о,
                    FormID = 'М',
                    LanguageID = "RU"
                });
            }

            return nameForms;
        }
    }
}