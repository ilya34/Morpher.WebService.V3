// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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