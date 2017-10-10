namespace Morpher.WebService.V3.General
{
    using System;

    public enum CorrectionLanguage
    {
        Russian,
        Ukrainian
    }

    public static class CorrectionLanguageExtensions
    {
        public static string ToDatabaseLanguage(this CorrectionLanguage language)
        {
            switch (language)
            {
                case CorrectionLanguage.Russian: return "RU";
                case CorrectionLanguage.Ukrainian: return "UK";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }
    }
}