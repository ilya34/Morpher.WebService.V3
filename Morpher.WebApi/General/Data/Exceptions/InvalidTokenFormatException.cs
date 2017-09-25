namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Net;

    public class InvalidTokenFormatException : MorpherException
    {
        private static readonly string ErrorMessage = "Неверный формат токена.";

        private static readonly HttpStatusCode ResponseWith = (HttpStatusCode)498;

        public InvalidTokenFormatException()
            : base(ErrorMessage, 10)
        {
            Code = 10;
            ResponseCode = ResponseWith;
        }

        public InvalidTokenFormatException(string message, int code, Exception inner = null)
            : base(message, code, inner)
        {
            Code = code;
            ResponseCode = ResponseWith;
        }
    }
}