namespace Morpher.WebService.V3.General
{
    using System;
    using System.Net;

    public class InvalidFlagsException : MorpherException
    {
        private static readonly string ErrorMessage =
            "Указаны неправильные флаги.";

        private static readonly HttpStatusCode ResponseWith = (HttpStatusCode)494;

        public InvalidFlagsException()
            : base(ErrorMessage, 12)
        {
            Code = 12;
            ResponseCode = ResponseWith;
        }


        public InvalidFlagsException(string message, int code, Exception inner = null)
            : base(message, code, inner)
        {
            Code = code;
            ResponseCode = ResponseWith;
        }
    }
}