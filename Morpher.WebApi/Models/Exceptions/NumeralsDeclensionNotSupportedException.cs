namespace Morpher.WebService.V3.Models.Exceptions
{
    using System;

    public class NumeralsDeclensionNotSupportedException : MorpherException
    {
        private static readonly string ErrorMessage =
            "Склонение числительных в declension не поддерживается. Используйте метод spell.";

        public NumeralsDeclensionNotSupportedException()
            : base(ErrorMessage, 4)
        {
            this.Code = 4;
        }

        public NumeralsDeclensionNotSupportedException(string mesage, int code)
            : base(ErrorMessage, code)
        {
            this.Code = code;
        }

        public NumeralsDeclensionNotSupportedException(string message, int code, Exception innerException)
            : base(ErrorMessage, code, innerException)
        {
            this.Code = code;
        }
    }
}