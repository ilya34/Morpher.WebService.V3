namespace Morpher.WebService.V3.Russian.Data
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using General.Data;

    [XmlRoot("DeclensionList")]
    public class DeclensionListResultXml
    {
        [XmlElement("DeclensionResult", typeof(DeclensionResult))]
        [XmlElement("Error", typeof(ServiceErrorMessage))]
        public List<object> Result = new List<object>();

        public static DeclensionListResultXml InflectList(
            Func<string, DeclensionFlags?, DeclensionResult> inflector,
            IEnumerable<string> words,
            DeclensionFlags? flags = null)
        {
            DeclensionListResultXml declensionListResultXml = new DeclensionListResultXml();

            foreach (var word in words)
            {
                try
                {
                    declensionListResultXml.Result.Add(inflector(word, flags));
                }
                catch (MorpherException exc)
                {
                    declensionListResultXml.Result.Add(new ServiceErrorMessage(exc));
                }
            }

            return declensionListResultXml;
        }
    }
}