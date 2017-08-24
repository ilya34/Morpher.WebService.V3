namespace Morpher.WebService.V3.Models.Exceptions
{
    using System;

    public class NotPayedException : MorpherException
    {
        private static readonly string ErrorMessage =
            "Необходимо оплатить услугу.";

        public NotPayedException()
            : base(ErrorMessage, 7)
        {
            Code = 7;
        }

        public NotPayedException(string message, int code)
            : base(message, code)
        {
            Code = code;
        }

        public NotPayedException(string message, int code, Exception exception)
            : base(message, code, exception)
        {
            Code = code;
        }
    }
}