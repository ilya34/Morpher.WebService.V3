namespace Morpher.WebService.V3.Models.Exceptions
{
    using System;

    public class RequiredParameterIsNotSpecified : MorpherException
    {
        private static readonly string ErrorMessage =
            "Не указан обязательный параметр: ";

        public RequiredParameterIsNotSpecified(string parameterName)
            : base(ErrorMessage, 6)
        {
            this.Code = 6;
        }

        public RequiredParameterIsNotSpecified(string message, int code)
            : base(message, code)
        {
            this.Code = code;
        }

        public RequiredParameterIsNotSpecified(string message, int code, Exception inner)
            : base(message, code, inner)
        {
            this.Code = code;
        }
    }
}