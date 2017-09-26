namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Net;

    public class IpBlockedException : MorpherException
    {
        private static readonly string ErrorMessage =
            "IP заблокирован.";

        private static readonly HttpStatusCode ResponseWith = HttpStatusCode.Forbidden;

        public IpBlockedException()
            : base(ErrorMessage, 3)
        {
            Code = 3;
            ResponseCode = ResponseWith;
        }

        public IpBlockedException(string message, int code, Exception inner = null)
            : base(message, code, inner)
        {
            Code = code;
            ResponseCode = ResponseWith;
        }
    }
}