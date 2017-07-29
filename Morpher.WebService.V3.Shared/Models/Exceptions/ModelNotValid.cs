// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Models.Exceptions
{
    using System;

    public class ModelNotValid : MorpherException
    {
        private static readonly string ErrorMessage =
            "Ошибка исправления: ";

        public ModelNotValid(string parameterName)
            : base(ErrorMessage + parameterName, 11)
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
