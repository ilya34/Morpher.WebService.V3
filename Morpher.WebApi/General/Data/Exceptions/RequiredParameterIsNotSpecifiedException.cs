namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Net;

    public class RequiredParameterIsNotSpecifiedException : MorpherException
    {
        private static readonly string ErrorMessage =
            "Не указан обязательный параметр: ";

        private static readonly HttpStatusCode ResponseWith = HttpStatusCode.BadRequest;

        public RequiredParameterIsNotSpecifiedException(string parameterName)
            : base(ErrorMessage + parameterName, 6)
        {
            Code = 6;
            ResponseCode = ResponseWith;
        }

        public RequiredParameterIsNotSpecifiedException(string message, int code = 6, Exception inner = null)
            : base(message, code, inner)
        {
            Code = code;
            ResponseCode = ResponseWith;
        }
    }
}