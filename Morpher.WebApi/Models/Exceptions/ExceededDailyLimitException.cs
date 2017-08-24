namespace Morpher.WebService.V3.Models.Exceptions
{
    using System;

    public class ExceededDailyLimitException : MorpherException
    {
        private static readonly string ErrorMessage =
            "Превышен лимит на количество запросов в сутки. Перейдите на следующий тарифный план.";

        public ExceededDailyLimitException()
            : base(ErrorMessage, 1)
        {
            Code = 1;
        }

        public ExceededDailyLimitException(string message, int code)
            : base(message, code)
        {
            Code = code;
        }

        public ExceededDailyLimitException(string message, int code, Exception inner)
            : base(message, code, inner)
        {
            Code = code;
        }
    }
}