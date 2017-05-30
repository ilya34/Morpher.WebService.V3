namespace Morpher.WebService.V3.Models
{
    using System.Runtime.Serialization;

    public class CacheResetPostModel
    {
        public string ClientToken { get; set; }

        public string AdminPassword { get; set; }

        public ResponseFormat? Format { get; set; }
    }
}