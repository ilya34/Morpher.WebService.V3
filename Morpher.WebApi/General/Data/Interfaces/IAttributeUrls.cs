namespace Morpher.WebService.V3.General.Data
{
    using System.Collections.Generic;

    public interface IAttributeUrls
    {
        Dictionary<string, ThrottleThisAttribute> Urls { get; }
    }
}