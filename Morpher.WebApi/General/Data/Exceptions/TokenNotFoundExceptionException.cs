namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Net;

    public class TokenNotFoundExceptionException : MorpherException
    {
        private static readonly string ErrorMessage = "Данный token не найден";

        private static readonly HttpStatusCode ResponseWith = (HttpStatusCode)498;

        public TokenNotFoundExceptionException()
            : base(ErrorMessage, 9)
        {
            Code = 9;
            ResponseCode = ResponseWith;
        }

        public TokenNotFoundExceptionException(string mesage, int code, Exception innerException = null)
            : base(mesage, code, innerException)
        {
            Code = code;
            ResponseCode = ResponseWith;
        }
    }
}