namespace Morpher.WebService.V3.General.Data
{
    using System;

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

        public MorpherException(string message, int code, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }

        public int Code { get; protected set; }
    }
}