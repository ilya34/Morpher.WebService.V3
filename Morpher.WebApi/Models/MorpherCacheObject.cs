namespace Morpher.WebApi.Models
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "StyleCop.SA1201")]
    [SuppressMessage("ReSharper", "StyleCop.SA1401")]
    public class MorpherCacheObject
    {
        public bool PaidUser { get; set; }

        public bool Unlimited { get; set; }

        public int QueriesLeft;
    }
}