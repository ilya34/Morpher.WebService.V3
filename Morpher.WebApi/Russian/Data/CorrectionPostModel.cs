namespace Morpher.WebService.V3.Russian.Data
{
    using System.Collections.Generic;
    using General;
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
        public string М { get; set; }
        public string М_И { get; set; }
        public string М_Р { get; set; }
        public string М_Д { get; set; }
        public string М_В { get; set; }
        public string М_Т { get; set; }
        public string М_П { get; set; }
        public string М_М { get; set; }

        public List<NameForm> ToNameForms()
        {
            List<NameForm> nameForms = new List<NameForm>();

            Add(nameForms, И, 'И');
            Add(nameForms, Р, 'Р');
            Add(nameForms, Д, 'Д');
            Add(nameForms, В, 'В');
            Add(nameForms, Т, 'Т');
            Add(nameForms, П, 'П');
            Add(nameForms, М, 'М');
            Add(nameForms, М_И, 'И', true);
            Add(nameForms, М_Р, 'Р', true);
            Add(nameForms, М_Д, 'Д', true);
            Add(nameForms, М_В, 'В', true);
            Add(nameForms, М_Т, 'Т', true);
            Add(nameForms, М_П, 'П', true);
            Add(nameForms, М_М, 'М', true);

            return nameForms;
        }

        static void Add(List<NameForm> nameForms, string form, char formID, bool plural = false)
        {
            if (!string.IsNullOrWhiteSpace(form))
            {
                nameForms.Add(new NameForm()
                {
                    Plural = plural,
                    AccentedText = form,
                    FormID = formID,
                    LanguageID = CorrectionLanguage.Russian.ToDatabaseLanguage()
                });
            }
        }
    }
}