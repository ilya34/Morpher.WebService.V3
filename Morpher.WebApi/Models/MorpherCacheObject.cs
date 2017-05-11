namespace Morpher.WebApi.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "StyleCop.SA1201")]
    [SuppressMessage("ReSharper", "StyleCop.SA1401")]
    public class MorpherCacheObject
    {
        public Guid? UserId { get; set; }

        public bool PaidUser { get; set; }

        public bool Unlimited { get; set; }

        public int QueriesLeft;
    }
}