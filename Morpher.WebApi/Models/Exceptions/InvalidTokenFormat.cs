namespace Morpher.WebService.V3.Models.Exceptions
{
    using System;

    public class InvalidTokenFormat : MorpherException
    {
        private static readonly string ErrorMessage = "Неверный формат токена.";

        public InvalidTokenFormat()
            : base(ErrorMessage, 10)
        {
            Code = 10;
        }

        public InvalidTokenFormat(string message, int code)
            : base(message, code)
        {
            Code = code;
        }

        public InvalidTokenFormat(string message, int code, Exception inner)
            : base(message, code, inner)
        {
            Code = code;
        }
    }
}