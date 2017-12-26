namespace Morpher.WebService.V3.General
{
    using System;
    using System.Net;

    public class ExceededDailyLimitException : MorpherException
    {
        private static readonly string ErrorMessage =
            "Превышен лимит на количество запросов в сутки. Перейдите на следующий тарифный план.";

        private static readonly HttpStatusCode ResponseWith = HttpStatusCode.PaymentRequired;

        public ExceededDailyLimitException()
            : base(ErrorMessage, 1)
        {
            Code = 1;
            ResponseCode = ResponseWith;
        }


        public ExceededDailyLimitException(string message, int code, Exception inner = null)
            : base(message, code, inner)
        {
            Code = code;
            ResponseCode = ResponseWith;
        }
    }
}