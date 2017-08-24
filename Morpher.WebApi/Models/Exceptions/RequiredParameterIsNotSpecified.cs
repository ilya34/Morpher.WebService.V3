namespace Morpher.WebService.V3.Models.Exceptions
{
    using System;

    public class RequiredParameterIsNotSpecified : MorpherException
    {
        private static readonly string ErrorMessage =
            "Не указан обязательный параметр: ";

        public RequiredParameterIsNotSpecified(string parameterName)
            : base(ErrorMessage + parameterName, 6)
        {
            Code = 6;
        }

        public RequiredParameterIsNotSpecified(string message, int code, Exception inner = null)
            : base(message, code, inner)
        {
            Code = code;
        }
    }
}