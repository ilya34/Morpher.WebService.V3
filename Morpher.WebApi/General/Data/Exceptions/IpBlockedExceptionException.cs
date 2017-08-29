namespace Morpher.WebService.V3.General.Data
{
    using System;

    public class IpBlockedExceptionException : MorpherException
    {
        private static readonly string ErrorMessage =
            "IP заблокирован.";

        public IpBlockedExceptionException()
            : base(ErrorMessage, 3)
        {
            Code = 3;
        }

        public IpBlockedExceptionException(string message, int code)
            : base(message, code)
        {
            Code = code;
        }

        public IpBlockedExceptionException(string message, int code, Exception inner)
            : base(message, code, inner)
        {
            Code = code;
        }
    }
}