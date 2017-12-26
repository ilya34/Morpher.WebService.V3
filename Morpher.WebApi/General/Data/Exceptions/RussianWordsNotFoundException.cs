namespace Morpher.WebService.V3.General
{
    using System;
    using System.Net;

    public class RussianWordsNotFoundException : MorpherException
    {
        private static readonly string ErrorMessage = "Не найдено русских слов.";

        private static readonly HttpStatusCode ResponseWith = (HttpStatusCode)496;

        public RussianWordsNotFoundException()
            : base(ErrorMessage, 5)
        {
            Code = 5;
            ResponseCode = ResponseWith;
        }

        public RussianWordsNotFoundException(string message, int code, Exception innerException = null)
            : base(message, code, innerException)
        {
            Code = code;
            ResponseCode = ResponseWith;
        }
    }
}