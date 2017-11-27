namespace Morpher.WebService.V3.Russian.Data
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using General.Data;

    [DataContract]
    public class DeclensionListResultJson
    {
        public DeclensionListResultJson()
        {
            
        }

        public DeclensionListResultJson(DeclensionResult result)
        {
            DeclensionResult = result;
        }

        public DeclensionListResultJson(ServiceErrorMessage message)
        {
            ServiceErrorMessage = message;
        }

        [DataMember(EmitDefaultValue = false)]
        public DeclensionResult  DeclensionResult { get; set; }

        [DataMember(Name = "Error", EmitDefaultValue = false)]
        public ServiceErrorMessage ServiceErrorMessage { get; set; }

        public static List<DeclensionListResultJson> InflectList(
            Func<string, DeclensionFlags?, DeclensionResult> inflector,
            IEnumerable<string> words,
            DeclensionFlags? flags = null)
        {
            List<DeclensionListResultJson> list = new List<DeclensionListResultJson>();
            foreach (var word in words)
            {
                try
                {
                    var inflected = inflector(word, flags);
                    list.Add(new DeclensionListResultJson(inflected));
                }
                catch (MorpherException exc)
                {
                    list.Add(new DeclensionListResultJson(new ServiceErrorMessage(exc)));
                }
            }

            return list;
        }
    }
}