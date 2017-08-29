namespace Morpher.WebService.V3.General.Data
{
    using System;

    public class RussianWordsNotFoundException : MorpherException
    {
        private static readonly string ErrorMessage = "Не найдено русских слов.";

        public RussianWordsNotFoundException()
            : base(ErrorMessage, 5)
        {
            Code = 5;
        }

        public RussianWordsNotFoundException(string message, int code)
            : base(message, code)
        {
            Code = code;
        }

        public RussianWordsNotFoundException(string message, int code, Exception innerException)
            : base(message, code, innerException)
        {
            Code = code;
        }
    }
}