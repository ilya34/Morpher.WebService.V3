namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Net;

    public class NumeralsDeclensionNotSupportedException : MorpherException
    {
        private static readonly string ErrorMessage =
            "Склонение числительных в declension не поддерживается. Используйте метод spell.";

        private static readonly HttpStatusCode ResponseWith = (HttpStatusCode)495;

        public NumeralsDeclensionNotSupportedException()
            : base(ErrorMessage, 4)
        {
            Code = 4;
            ResponseCode = ResponseWith;
        }


        public NumeralsDeclensionNotSupportedException(string message, int code, Exception innerException = null)
            : base(message, code, innerException)
        {
            Code = code;
            ResponseCode = ResponseWith;
        }
    }
}