namespace Morpher.WebService.V3.General.Data
{
    using System;

    public class InvalidTokenFormatException : MorpherException
    {
        private static readonly string ErrorMessage = "Неверный формат токена.";

        public InvalidTokenFormatException()
            : base(ErrorMessage, 10)
        {
            Code = 10;
        }

        public InvalidTokenFormatException(string message, int code)
            : base(message, code)
        {
            Code = code;
        }

        public InvalidTokenFormatException(string message, int code, Exception inner)
            : base(message, code, inner)
        {
            Code = code;
        }
    }
}