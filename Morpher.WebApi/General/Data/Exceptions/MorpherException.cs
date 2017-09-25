namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Net;

    public class MorpherException : Exception
    {
        public MorpherException()
        {
        }

        public MorpherException(int code)
        {
            Code = code;
        }

        public MorpherException(string message, int code)
            : base(message)
        {
            Code = code;
        }

        public MorpherException(string message, int code, Exception innerException = null)
            : base(message, innerException)
        {
            Code = code;
        }

        public int Code { get; protected set; }

        public HttpStatusCode ResponseCode { get; protected set; }
    }
}