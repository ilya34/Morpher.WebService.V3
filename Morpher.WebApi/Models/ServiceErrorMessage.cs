namespace Morpher.WebService.V3.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using Exceptions;

    [DataContract(Name = "error")]
    public class ServiceErrorMessage : IEquatable<ServiceErrorMessage>
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

        [SuppressMessage("ReSharper", "StyleCop.SA1126")]
        public static bool operator ==(ServiceErrorMessage left, ServiceErrorMessage right)
        {
            return Equals(left, right);
        }

        [SuppressMessage("ReSharper", "StyleCop.SA1126")]
        public static bool operator !=(ServiceErrorMessage left, ServiceErrorMessage right)
        {
            return !Equals(left, right);
        }

        [SuppressMessage("ReSharper", "StyleCop.SA1503")]
        public bool Equals(ServiceErrorMessage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Code == other.Code && string.Equals(Message, other.Message);
        }

        [SuppressMessage("ReSharper", "StyleCop.SA1503")]
        [SuppressMessage("ReSharper", "StyleCop.SA1126")]
        [SuppressMessage("ReSharper", "ArrangeThisQualifier")]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((ServiceErrorMessage)obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                return (Code * 397) ^ (Message != null ? Message.GetHashCode() : 0);
            }
        }
    }
}