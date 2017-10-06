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

            GetValue(nameForms, model.Н, 'Н');
            GetValue(nameForms, model.Р, 'Р');
            GetValue(nameForms, model.Д, 'Д');
            GetValue(nameForms, model.З, 'З');
            GetValue(nameForms, model.О, 'О');
            GetValue(nameForms, model.М, 'М');
            GetValue(nameForms, model.К, 'К');
            GetValue(nameForms, model.М_Н, 'Н', true);
            GetValue(nameForms, model.М_Р, 'Р', true);
            GetValue(nameForms, model.М_Д, 'Д', true);
            GetValue(nameForms, model.М_З, 'З', true);
            GetValue(nameForms, model.М_О, 'О', true);
            GetValue(nameForms, model.М_М, 'М', true);
            GetValue(nameForms, model.М_К, 'К', true);

            return nameForms;
        }

        static void GetValue(List<NameForm> nameForms, string form, char formID, bool plural = false)
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