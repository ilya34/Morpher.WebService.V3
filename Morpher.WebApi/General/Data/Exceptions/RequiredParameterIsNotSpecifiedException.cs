namespace Morpher.WebService.V3.General.Data
{
    using System;

    public class RequiredParameterIsNotSpecifiedException : MorpherException
    {
        private static readonly string ErrorMessage =
            "Не указан обязательный параметр: ";

        public RequiredParameterIsNotSpecifiedException(string parameterName)
            : base(ErrorMessage + parameterName, 6)
        {
            Code = 6;
        }

        public RequiredParameterIsNotSpecifiedException(string message, int code, Exception inner = null)
            : base(message, code, inner)
        {
            Code = code;
        }
    }
}