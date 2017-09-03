namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract(Name = "error")]
    public class ServiceErrorMessage
    { 
        public ServiceErrorMessage(MorpherException exception)
        {
            Code = exception.Code;
            Message = exception.Message;
        }

        [DataMember(Name = "code", Order = 0)]
        public int Code { get; protected set; }

        [DataMember(Name = "message", Order = 1)]
        public string Message { get; protected set; }
    }
}