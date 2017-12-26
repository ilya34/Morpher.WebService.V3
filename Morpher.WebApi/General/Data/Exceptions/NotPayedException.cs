namespace Morpher.WebService.V3.General
{
    using System;
    using System.Net;

    public class NotPayedException : MorpherException
    {
        private static readonly string ErrorMessage =
            "Необходимо оплатить услугу.";

        private static readonly HttpStatusCode ResponseWith = HttpStatusCode.PaymentRequired;

        public NotPayedException()
            : base(ErrorMessage, 7)
        {
            Code = 7;
            ResponseCode = ResponseWith;
        }

        public NotPayedException(string message, int code, Exception exception = null)
            : base(message, code, exception)
        {
            Code = code;
            ResponseCode = ResponseWith;
        }
    }
}