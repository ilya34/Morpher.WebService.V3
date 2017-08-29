namespace Morpher.WebService.V3.General.Data
{
    using System;

    public class TokenNotFoundExceptionException : MorpherException
    {
        private static readonly string ErrorMessage = "Данный token не найден";

        public TokenNotFoundExceptionException()
            : base(ErrorMessage, 9)
        {
            Code = 9;
        }

        public TokenNotFoundExceptionException(string message, int code)
            : base(message, code)
        {
            Code = code;
        }

        public TokenNotFoundExceptionException(string mesage, int code, Exception innerException)
            : base(mesage, code, innerException)
        {
            Code = code;
        }
    }
}