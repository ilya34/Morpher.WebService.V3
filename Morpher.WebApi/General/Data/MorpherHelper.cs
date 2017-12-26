using System;

namespace Morpher.WebService.V3.General
{
    public static class MorpherHelper
    {
        public static Exception MapClientExceptionIfPossible(Exception exc)
        {
            if (exc is DailyLimitExceededException)
                return new ExceededDailyLimitException();
            if (exc is InvalidFlagsException)
                return new InvalidFlagsException();
            if (exc is V3.IpBlockedException)
                return new V3.IpBlockedException();

            return exc;
        }

        public static Russian.DeclensionFlags? MapFlags(this DeclensionFlags? flags)
        {
            if (flags == null) return null;

            Russian.DeclensionFlags serviceFlags = 0;
            if ((flags & DeclensionFlags.Name) != 0)
                serviceFlags |= Russian.DeclensionFlags.Name;

            if ((flags & DeclensionFlags.Common) != 0)
                serviceFlags |= Russian.DeclensionFlags.Common;

            if ((flags & DeclensionFlags.Feminine) != 0)
                serviceFlags |= Russian.DeclensionFlags.Feminine;

            if ((flags & DeclensionFlags.Masculine) != 0)
                serviceFlags |= Russian.DeclensionFlags.Masculine;

            if ((flags & DeclensionFlags.Neuter) != 0)
                serviceFlags |= Russian.DeclensionFlags.Neuter;

            if ((flags & DeclensionFlags.Animate) != 0)
                serviceFlags |= Russian.DeclensionFlags.Animate;

            if ((flags & DeclensionFlags.Inanimate) != 0)
                serviceFlags |= Russian.DeclensionFlags.Inanimate;

            if ((flags & DeclensionFlags.Plural) != 0)
                serviceFlags |= Russian.DeclensionFlags.Plural;

            return serviceFlags;
        }
    }
}