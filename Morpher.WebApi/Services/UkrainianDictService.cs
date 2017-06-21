namespace Morpher.WebService.V3.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml.Serialization;

    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Models;

    public class UkrainianDictService : IUkrainianDictService
    {
        private static readonly XmlSerializer XmlSerializer =
            new XmlSerializer(typeof(List<UkrainianEntry>), new XmlRootAttribute("dictionary"));

        private readonly string filePath = HttpContext.Current.Server.MapPath("~/App_Data/UserDictUkr.xml");

        private readonly Dictionary<string, UkrainianEntry> ukrainianDictionary = new Dictionary<string, UkrainianEntry>();

        public static List<UkrainianEntry> LoadFromXml(TextReader reader)
        {
            return (List<UkrainianEntry>)XmlSerializer.Deserialize(reader);
        }

        public static void Save(List<UkrainianEntry> entries, TextWriter textWriter)
        {
            XmlSerializer.Serialize(textWriter, entries);
        }

        public UkrainianDeclensionForms Get(string nominativeSingular, bool plural)
        {
            UkrainianEntry ukrainianEntry;
            this.ukrainianDictionary.TryGetValue(nominativeSingular.ToUpperInvariant(), out ukrainianEntry);
            return !plural ? ukrainianEntry?.Singular : ukrainianEntry?.Plural;
        }

        public List<UkrainianEntry> GetAll()
        {
            return this.ukrainianDictionary.Values.ToList();
        }

        public void Load(List<UkrainianEntry> entries)
        {
            foreach (var entry in entries)
            {
                this.Add(entry);
            }
        }

        public void AddOrUpdate(UkrainianEntry ukrainianEntry)
        {
            UkrainianEntry ukrainianDictEntry;
            if (this.ukrainianDictionary.TryGetValue(ukrainianEntry.Singular.Nominative.ToUpperInvariant(), out ukrainianDictEntry))
            {
                ukrainianDictEntry.Singular.AddOrUpdate(ukrainianEntry.Singular);
                if (ukrainianEntry.Plural != null)
                {
                    if (ukrainianDictEntry.Plural == null)
                    {
                        ukrainianDictEntry.Plural = new UkrainianDeclensionForms();
                    }

                    ukrainianDictEntry.Plural.AddOrUpdate(ukrainianEntry.Plural);
                }
            }
            else
            {
                this.ukrainianDictionary.Add(ukrainianEntry.Singular.Nominative.ToUpperInvariant(), ukrainianEntry);
            }

            using (StreamWriter streamWriter = new StreamWriter(this.filePath))
            {
                Save(this.ukrainianDictionary.Values.ToList(), streamWriter);
            }
        }

        public void Remove(string singularNominative)
        {
            this.ukrainianDictionary.Remove(singularNominative.ToUpperInvariant());
        }

        private void Add(UkrainianEntry ukrainianEntry)
        {
            this.ukrainianDictionary.Add(ukrainianEntry.Singular.Nominative.ToUpperInvariant(), ukrainianEntry);
        }
    }
}