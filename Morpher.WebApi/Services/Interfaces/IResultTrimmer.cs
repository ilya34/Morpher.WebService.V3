namespace Morpher.WebService.V3.Services.Interfaces
{
    using System;

    public interface IResultTrimmer
    {
        void Trim(object obj);
        void Trim(object obj, Guid? token);
    }
}