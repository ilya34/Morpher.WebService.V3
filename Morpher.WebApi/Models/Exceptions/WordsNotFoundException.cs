namespace Morpher.WebApi.Models.Exceptions
{
    using System;

    public class WordsNotFoundException : MorpherException
    {
        private static readonly string ErrorMessage = "Данное слово не найдено.";

        public WordsNotFoundException()
            : base(ErrorMessage, 5)
        {
            this.Code = 5;
        }

        public WordsNotFoundException(string message, int code)
            : base(message, code)
        {
            this.Code = code;
        }

        public WordsNotFoundException(string message, int code, Exception innerException)
            : base(message, code, innerException)
        {
            this.Code = code;
        }
    }
}