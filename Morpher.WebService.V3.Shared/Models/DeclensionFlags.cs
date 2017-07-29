// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Flags]
    [SuppressMessage("ReSharper", "StyleCop.SA1602")]
    public enum DeclensionFlags
    {
        Name = 1 << 0,
        Common = 1 << 1,
        Feminine = 1 << 2,
        Masculine = 1 << 3,
        Neuter = 1 << 4,
        Animate = 1 << 5,
        Inanimate = 1 << 6,
        Plural = 1 << 7
    }
}