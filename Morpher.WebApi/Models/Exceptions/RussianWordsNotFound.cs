namespace Morpher.WebService.V3.Models.Exceptions
{
    using System;

    public class RussianWordsNotFound : MorpherException
    {
        private static readonly string ErrorMessage = "Не найдено русских слов.";

        public RussianWordsNotFound()
            : base(ErrorMessage, 5)
        {
            Code = 5;
        }

        public RussianWordsNotFound(string message, int code)
            : base(message, code)
        {
            Code = code;
        }

        public RussianWordsNotFound(string message, int code, Exception innerException)
            : base(message, code, innerException)
        {
            Code = code;
        }
    }
}