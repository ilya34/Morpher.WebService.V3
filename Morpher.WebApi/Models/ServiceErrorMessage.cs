namespace Morpher.WebApi.Models
{
    using System.Runtime.Serialization;

    using Morpher.WebApi.Models.Exceptions;

    [DataContract(Name = "error")]
    public class ServiceErrorMessage
    {
        public ServiceErrorMessage(MorpherException exception)
        {
            this.Code = exception.Code;
            this.Message = exception.Message;
        }

        [DataMember(Name = "code", Order = 0)]
        public int Code { get; protected set; }

        [DataMember(Name = "message", Order = 1)]
        public string Message { get; protected set; }
    }
}