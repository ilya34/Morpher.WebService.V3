namespace Morpher.WebService.V3.General.Data
{
    using System;

    public class NumeralsDeclensionNotSupportedException : MorpherException
    {
        private static readonly string ErrorMessage =
            "Склонение числительных в declension не поддерживается. Используйте метод spell.";

        public NumeralsDeclensionNotSupportedException()
            : base(ErrorMessage, 4)
        {
            Code = 4;
        }

        public NumeralsDeclensionNotSupportedException(string mesage, int code)
            : base(ErrorMessage, code)
        {
            Code = code;
        }

        public NumeralsDeclensionNotSupportedException(string message, int code, Exception innerException)
            : base(ErrorMessage, code, innerException)
        {
            Code = code;
        }
    }
}