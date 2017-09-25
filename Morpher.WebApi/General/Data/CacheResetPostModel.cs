namespace Morpher.WebService.V3.General.Data
{
    public class CacheResetPostModel
    {
        public string ClientToken { get; set; }

        public string AdminPassword { get; set; }

        public ResponseFormat? Format { get; set; }
    }
}