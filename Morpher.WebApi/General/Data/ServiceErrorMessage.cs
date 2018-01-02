using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Morpher.WebService.V3.General.Data
{
    using System.Xml.Serialization;
    using Exceptions;

    [XmlRoot("error")]
    public class ServiceErrorMessage
    {
        public ServiceErrorMessage()
        {
        }

        public ServiceErrorMessage(MorpherException exception)
        {
            Code = exception.Code;
            Message = exception.Message;
        }

        public ServiceErrorMessage(ServerException exception)
        {
            Code = exception.Code;
            Message = exception.Message;
        }

        [JsonProperty(PropertyName = "code", Order = 0)]
        [XmlElement("code", Order = 0)]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "message", Order = 1)]
        [XmlElement("message", Order = 1)]
        public string Message { get; set; }
    }
}