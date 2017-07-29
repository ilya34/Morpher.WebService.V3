// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Models.Exceptions
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

        public NumeralsDeclensionNotSupportedException(string message, int code)
            : base(message, code)
        {
            this.Code = code;
        }

        public NumeralsDeclensionNotSupportedException(string message, int code, Exception innerException)
            : base(message, code, innerException)
        {
            this.Code = code;
        }
    }
}