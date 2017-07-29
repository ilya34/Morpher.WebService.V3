// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Models.Exceptions
{
    using System;

    public class MorpherException : Exception
    {
        public MorpherException()
        {
        }

        public MorpherException(int code)
        {
            this.Code = code;
        }

        public MorpherException(string message, int code)
            : base(message)
        {
            this.Code = code;
        }

        public MorpherException(string message, int code, Exception innerException)
            : base(message, innerException)
        {
            this.Code = code;
        }

        public int Code { get; protected set; }
    }
}