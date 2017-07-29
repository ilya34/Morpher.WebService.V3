// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Models
{
    public class CacheResetPostModel
    {
        public string ClientToken { get; set; }

        public string AdminPassword { get; set; }

        public ResponseFormat? Format { get; set; }
    }
}