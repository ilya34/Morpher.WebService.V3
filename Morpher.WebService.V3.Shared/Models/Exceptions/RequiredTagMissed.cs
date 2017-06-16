namespace Morpher.WebService.V3.Shared.Models.Exceptions
{
    using System;

    public class ModelNotValid : MorpherException
    {
        private static readonly string ErrorMessage =
            "Ошибка исправления: ";

        public ModelNotValid(string parameterName)
            : base(ErrorMessage, 11)
        {
            this.Code = 11;
        }

        public ModelNotValid(string message, int code)
            : base(message, code)
        {
            this.Code = code;
        }

        public ModelNotValid(string message, int code, Exception inner)
            : base(message, code, inner)
        {
            this.Code = code;
        }
    }
}
