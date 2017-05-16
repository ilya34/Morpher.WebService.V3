namespace Morpher.WebService.V3.Models.Exceptions
{
    using System;

    public class TokenNotFoundException : MorpherException
    {
        private static readonly string ErrorMessage = "Данный token не найден";

        public TokenNotFoundException()
            : base(ErrorMessage, 9)
        {
            this.Code = 9;
        }

        public TokenNotFoundException(string message, int code)
            : base(message, code)
        {
            this.Code = code;
        }

        public TokenNotFoundException(string mesage, int code, Exception innerException)
            : base(mesage, code, innerException)
        {
            this.Code = code;
        }
    }
}