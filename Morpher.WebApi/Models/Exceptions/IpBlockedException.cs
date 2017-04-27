namespace Morpher.WebApi.Models.Exceptions
{
    using System;

    public class IpBlockedException : MorpherException
    {
        private static readonly string ErrorMessage =
            "IP заблокирован.";

        public IpBlockedException()
            : base(ErrorMessage, 3)
        {
            this.Code = 3;
        }

        public IpBlockedException(string message, int code)
            : base(message, code)
        {
            this.Code = code;
        }

        public IpBlockedException(string message, int code, Exception inner)
            : base(message, code, inner)
        {
            this.Code = code;
        }
    }
}