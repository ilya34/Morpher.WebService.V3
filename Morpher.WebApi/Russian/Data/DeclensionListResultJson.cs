using Newtonsoft.Json;

namespace Morpher.WebService.V3.Russian.Data
{
    using System;
    using System.Collections.Generic;

    using General.Data;

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

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DeclensionResult  DeclensionResult { get; set; }

        [JsonProperty(PropertyName = "Error", DefaultValueHandling = DefaultValueHandling.Ignore)]
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