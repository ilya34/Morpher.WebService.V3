namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Net;

    public class TokenNotFoundException : MorpherException
    {
        private static readonly string ErrorMessage = "Данный token не найден";

        private static readonly HttpStatusCode ResponseWith = (HttpStatusCode)498;

        public TokenNotFoundException()
            : base(ErrorMessage, 9)
        {
            Code = 9;
            ResponseCode = ResponseWith;
        }

        public TokenNotFoundException(string mesage, int code, Exception innerException = null)
            : base(mesage, code, innerException)
        {
            Code = code;
            ResponseCode = ResponseWith;
        }
    }
}