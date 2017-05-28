namespace Morpher.WebService.V3.Shared.Models.Exceptions
{
    using System;

    public class MorpherException : Exception
    {
        public MorpherException()
        {
        }

        public MorpherException(int code)
        {
            this.Code = code;
        }

        public MorpherException(string message, int code)
            : base(message)
        {
            this.Code = code;
        }

        public MorpherException(string message, int code, Exception innerException)
            : base(message, innerException)
        {
            this.Code = code;
        }

        public int Code { get; protected set; }
    }
}