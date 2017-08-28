namespace Morpher.WebService.V3.Services
{
    using System;
    using Interfaces;

    public class DummyResultTrimmer : IResultTrimmer
    {
        public void Trim(object obj)
        {
            return;
        }

        public void Trim(object obj, Guid? token)
        {
            return;
        }
    }
}