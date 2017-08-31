namespace Morpher.WebService.V3.General
{
    using System.Text.RegularExpressions;

    public class LemmaNormalizer
    {
        static readonly Regex Regex = new Regex(@"[\s-,.]+", RegexOptions.Compiled);

        public static string Normalize(string s)
        {
            string upperInvariant =
                CompressWhitespace(s)
                    .Trim();

            return ToUpperRemoveAccents(upperInvariant);
        }

        public static string ToUpperRemoveAccents(string s)
        {
            string x = s
                .ToUpperInvariant()
                .Replace('Ё', 'Е');

            return RemoveAccents(x);
        }

        public static string RemoveAccents(string s)
        {
            return s
                .Replace("\u0300", "")  // combining grave accent 
                .Replace("\u0301", ""); // combining acute accent 
        }

        public static string CompressWhitespace(string s)
        {
            return Regex.Replace(s, " ");
        }
    }
}