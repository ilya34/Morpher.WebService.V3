using System;

namespace Morpher.WebService.V3.General.Data.Exceptions
{
    using System.Net;

    public class CorrectionPostModelIsEmptyException : MorpherException
    {
        private static readonly string ErrorMessage = "Не удалось получить данные. Проверьте правильность запроса.";

        private static readonly HttpStatusCode ResponseWith =  HttpStatusCode.BadRequest;

        public CorrectionPostModelIsEmptyException()
            : base(ErrorMessage, 11)
        {
            ResponseCode = ResponseWith;
        }

        public CorrectionPostModelIsEmptyException(string message, int code, Exception innerException = null)
            : base(message, code, innerException)
        {
            Code = code;
            ResponseCode = ResponseWith;
        }
    }
}