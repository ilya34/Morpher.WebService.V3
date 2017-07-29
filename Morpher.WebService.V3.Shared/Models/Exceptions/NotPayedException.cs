// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Models.Exceptions
{
    using System;

    public class NotPayedException : MorpherException
    {
        private static readonly string ErrorMessage =
            "Необходимо оплатить услугу.";

        public NotPayedException()
            : base(ErrorMessage, 7)
        {
            this.Code = 7;
        }

        public NotPayedException(string message, int code)
            : base(message, code)
        {
            this.Code = code;
        }

        public NotPayedException(string message, int code, Exception exception)
            : base(message, code, exception)
        {
            this.Code = code;
        }
    }
}