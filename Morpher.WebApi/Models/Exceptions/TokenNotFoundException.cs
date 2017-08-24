namespace Morpher.WebService.V3.Models.Exceptions
{
    using System;

    public class TokenNotFoundException : MorpherException
    {
        private static readonly string ErrorMessage = "Данный token не найден";

        public TokenNotFoundException()
            : base(ErrorMessage, 9)
        {
            Code = 9;
        }

        public TokenNotFoundException(string message, int code)
            : base(message, code)
        {
            Code = code;
        }

        public TokenNotFoundException(string mesage, int code, Exception innerException)
            : base(mesage, code, innerException)
        {
            Code = code;
        }
    }
}