using System;

namespace Morpher.WebService.V3.General.Data
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

        public static Russian.DeclensionFlags? MapFlags(this General.Data.DeclensionFlags? flags)
        {
            if (flags == null) return null;

            Russian.DeclensionFlags serviceFlags = 0;
            if ((flags & General.Data.DeclensionFlags.Name) != 0)
                serviceFlags |= Russian.DeclensionFlags.Name;

            if ((flags & General.Data.DeclensionFlags.Common) != 0)
                serviceFlags |= Russian.DeclensionFlags.Common;

            if ((flags & General.Data.DeclensionFlags.Feminine) != 0)
                serviceFlags |= Russian.DeclensionFlags.Feminine;

            if ((flags & General.Data.DeclensionFlags.Masculine) != 0)
                serviceFlags |= Russian.DeclensionFlags.Masculine;

            if ((flags & General.Data.DeclensionFlags.Neuter) != 0)
                serviceFlags |= Russian.DeclensionFlags.Neuter;

            if ((flags & General.Data.DeclensionFlags.Animate) != 0)
                serviceFlags |= Russian.DeclensionFlags.Animate;

            if ((flags & General.Data.DeclensionFlags.Inanimate) != 0)
                serviceFlags |= Russian.DeclensionFlags.Inanimate;

            if ((flags & General.Data.DeclensionFlags.Plural) != 0)
                serviceFlags |= Russian.DeclensionFlags.Plural;

            return serviceFlags;
        }
    }
}