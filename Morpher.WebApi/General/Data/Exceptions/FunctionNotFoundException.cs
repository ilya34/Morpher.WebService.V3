namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Net;

    public class FunctionNotFoundException : MorpherException
    {
        private static readonly string ErrorMessage = "Запрошенная функция не поддерживается";

        private static readonly HttpStatusCode ResponseWith = HttpStatusCode.NotImplemented;//http code = 501 

        public FunctionNotFoundException()
            : base(ErrorMessage, 13)
        {
            Code = 13;
            ResponseCode = ResponseWith;
        }

        public FunctionNotFoundException(string mesage, int code = 13, Exception innerException = null)
            : base(mesage, code, innerException)
        {
            Code = code;
            ResponseCode = ResponseWith;
        }
    }
}
