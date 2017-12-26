namespace Morpher.WebService.V3.General
{
    using System;
    using System.Net;

    public class ServerException : MorpherException
    {
        private static readonly string ErrorMessage =
            "Ошибка сервера";

        private static readonly HttpStatusCode ResponseWith = HttpStatusCode.InternalServerError;

        public ServerException()
            : base(ErrorMessage, 11)
        {
            Code = 11;
            ResponseCode = ResponseWith;
        }

        public ServerException(string message, int code)
            : base(message, code)
        {
            Code = code;
            ResponseCode = ResponseWith;
        }

        public ServerException(Exception inner)
            : base(ErrorMessage, 11, inner)
        {
            Code = 12;
            ResponseCode = ResponseWith;
        }
    }
}