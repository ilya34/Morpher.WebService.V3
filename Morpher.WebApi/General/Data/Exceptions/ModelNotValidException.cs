namespace Morpher.WebService.V3.General.Data
{
    using System;

    public class ModelNotValidException : MorpherException
    {
        private static readonly string ErrorMessage =
            "Ошибка исправления: ";

        public ModelNotValidException(string parameterName)
            : base(ErrorMessage + parameterName, 11)
        {
            Code = 11;
        }

        public ModelNotValidException(string message, int code)
            : base(message, code)
        {
            Code = code;
        }

        public ModelNotValidException(string message, int code, Exception inner)
            : base(message, code, inner)
        {
            Code = code;
        }
    }
}
