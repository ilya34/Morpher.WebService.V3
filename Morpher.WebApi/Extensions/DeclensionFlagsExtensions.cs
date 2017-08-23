namespace Morpher.WebService.V3.Extensions
{
    using System;
    using Models;

    public static class DeclensionFlagsExtensions
    {
        public static Russian.DeclensionFlags? ToServiceFlags(this DeclensionFlags? flags)
        {
            if (flags == null)
            {
                return null;
            }

            Russian.DeclensionFlags serviceFlags = default(Russian.DeclensionFlags);

            // защита от бинарной несовместимости
            if ((flags & DeclensionFlags.Plural) != 0)
            {
                serviceFlags |= Russian.DeclensionFlags.Plural;
            }

            if ((flags & DeclensionFlags.Animate) != 0)
            {
                serviceFlags |= Russian.DeclensionFlags.Animate;
            }

            if ((flags & DeclensionFlags.Common) != 0)
            {
                serviceFlags |= Russian.DeclensionFlags.Common;
            }

            if ((flags & DeclensionFlags.Feminine) != 0)
            {
                serviceFlags |= Russian.DeclensionFlags.Feminine;
            }

            if ((flags & DeclensionFlags.Inanimate) != 0)
            {
                serviceFlags |= Russian.DeclensionFlags.Inanimate;
            }

            if ((flags & DeclensionFlags.Masculine) != 0)
            {
                serviceFlags |= Russian.DeclensionFlags.Masculine;
            }

            if ((flags & DeclensionFlags.Name) != 0)
            {
                serviceFlags |= Russian.DeclensionFlags.Name;
            }

            if ((flags & DeclensionFlags.Neuter) != 0)
            {
                serviceFlags |= Russian.DeclensionFlags.Neuter;
            }

            return serviceFlags;
        }
    }
}