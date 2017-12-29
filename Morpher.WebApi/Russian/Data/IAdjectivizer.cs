using System.Collections.Generic;

namespace Morpher.WebService.V3.Russian.Data
{
    public interface IAdjectivizer
    {
        List<string> Adjectives(string s);
    }
}
