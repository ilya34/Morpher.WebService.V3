using System;

namespace Morpher.WebService.V3.General.Data.Exceptions
{
    public class CorrectionPostModelIsEmptyException : MorpherException
    {
        private static readonly string ErrorMessage = "Не удалось получить данные. Проверьте правильность запроса.";

        public CorrectionPostModelIsEmptyException()
            : base(ErrorMessage, 11)
        {
        }


        public CorrectionPostModelIsEmptyException(string message, int code) : base(message, code)
        {
            Code = code;
        }

        public CorrectionPostModelIsEmptyException(string message, int code, Exception innerException)
            : base(message, code, innerException)
        {
            Code = code;
        }
    }
}