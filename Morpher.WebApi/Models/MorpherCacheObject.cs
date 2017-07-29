// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Models
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