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

        public List<NameForm> ToNameForms()
        {
            List<NameForm> nameForms = new List<NameForm>();

            Add(nameForms, Н, 'Н');
            Add(nameForms, Р, 'Р');
            Add(nameForms, Д, 'Д');
            Add(nameForms, З, 'З');
            Add(nameForms, О, 'О');
            Add(nameForms, М, 'М');
            Add(nameForms, К, 'К');

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
                    LanguageID = CorrectionLanguage.Ukrainian.ToDatabaseLanguage()
                });
            }
        }
    }
}