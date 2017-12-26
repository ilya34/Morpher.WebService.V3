namespace Morpher.WebService.V3.General
{
    public class CacheResetPostModel
    {
        public string ClientToken { get; set; }

        public string AdminPassword { get; set; }

        public ResponseFormat? Format { get; set; }
    }
}