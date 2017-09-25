namespace Morpher.WebService.V3.General.Data.Services
{
    using System;

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