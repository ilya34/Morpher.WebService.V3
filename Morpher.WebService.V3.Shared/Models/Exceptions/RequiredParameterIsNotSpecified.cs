namespace Morpher.WebService.V3.Shared.Models.Exceptions
{
    using System;

    public class RequiredParameterIsNotSpecified : MorpherException
    {
        private static readonly string ErrorMessage =
            "Не указан обязательный параметр: ";

        public RequiredParameterIsNotSpecified(string parameterName)
            : base(ErrorMessage + parameterName, 6)
        {
            this.Code = 6;
        }

        public RequiredParameterIsNotSpecified(string message, int code, Exception inner = null)
            : base(message, code, inner)
        {
            this.Code = code;
        }
    }
}