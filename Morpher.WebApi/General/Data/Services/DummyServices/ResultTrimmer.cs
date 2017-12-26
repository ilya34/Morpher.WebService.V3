using System;

namespace Morpher.WebService.V3.General.DummyServices
{
    public class ResultTrimmer : IResultTrimmer
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