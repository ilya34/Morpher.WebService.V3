namespace Morpher.WebService.V3.General.Data
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Exceptions;

    [DataContract(Name = "error")]
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

        [DataMember(Name = "code", Order = 0)]
        [XmlElement("code")]
        public int Code { get; set; }

        [DataMember(Name = "message", Order = 1)]
        [XmlElement("message")]
        public string Message { get; set; }
    }
}