namespace Morpher.WebService.V3.Models.Exceptions
{
    using System;

    public class ModelNotValid : MorpherException
    {
        private static readonly string ErrorMessage =
            "Ошибка исправления: ";

        public ModelNotValid(string parameterName)
            : base(ErrorMessage + parameterName, 11)
        {
            Code = 11;
        }

        public ModelNotValid(string message, int code)
            : base(message, code)
        {
            Code = code;
        }

        public ModelNotValid(string message, int code, Exception inner)
            : base(message, code, inner)
        {
            Code = code;
        }
    }
}
