namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml.Serialization;

    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Models;

    public class RussianDictService : IRussianDictService
    {
        private static readonly XmlSerializer XmlSerializer =
            new XmlSerializer(typeof(List<RussianEntry>), new XmlRootAttribute("dictionary"));

        private readonly string filePath = HttpContext.Current.Server.MapPath("~/App_Data/UserDict.xml");

        private readonly Dictionary<string, RussianEntry> russianDictionary = new Dictionary<string, RussianEntry>();

        public static List<RussianEntry> LoadFromXml(TextReader reader)
        {
            return (List<RussianEntry>)XmlSerializer.Deserialize(reader);
        }

        public static void Save(List<RussianEntry> entries, TextWriter textWriter)
        {
            XmlSerializer.Serialize(textWriter, entries);
        }

        public RussianDeclensionForms Get(string nominativeSingular, bool plural)
        {
            RussianEntry russianEntry;
            this.russianDictionary.TryGetValue(nominativeSingular.ToUpperInvariant(), out russianEntry);
            return !plural ? russianEntry?.Singular : russianEntry?.Plural;
        }

        public List<RussianEntry> GetAll()
        {
            return this.russianDictionary.Values.ToList();
        }

        public void Load(List<RussianEntry> entries)
        {
            foreach (var entry in entries)
            {
                this.Add(entry);
            }
        }

        public void AddOrUpdate(RussianEntry russianEntry)
        {
            RussianEntry dictRussianEntry;
            if (this.russianDictionary.TryGetValue(russianEntry.Singular.Nominative.ToUpperInvariant(), out dictRussianEntry))
            {
                dictRussianEntry.Singular.AddOrUpdate(russianEntry.Singular);
                if (russianEntry.Plural != null)
                {
                    if (dictRussianEntry.Plural == null)
                    {
                        dictRussianEntry.Plural = new RussianDeclensionForms();
                    }

                    dictRussianEntry.Plural.AddOrUpdate(russianEntry.Plural);
                }
            }
            else
            {
                this.russianDictionary.Add(russianEntry.Singular.Nominative.ToUpperInvariant(), russianEntry);
            }

            using (StreamWriter streamWriter = new StreamWriter(this.filePath))
            {
                Save(this.russianDictionary.Values.ToList(), streamWriter);
            }
        }

        public void Remove(string singularNominative)
        {
            this.russianDictionary.Remove(singularNominative.ToUpperInvariant());
        }

        private void Add(RussianEntry russianEntry)
        {
            this.russianDictionary.Add(russianEntry.Singular.Nominative.ToUpperInvariant(), russianEntry);
        }
    }
}