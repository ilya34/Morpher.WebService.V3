namespace Morpher.WebService.V3.General.Data
{
    using System;

    public enum TarificationMode
    {
        PerRequest,
        PerSymbol,
        PerWord
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ThrottleThisAttribute : Attribute
    {

        public ThrottleThisAttribute(int cost = 1)
        {
            Mode = TarificationMode.PerRequest;
            Cost = cost;
        }

        /// <summary>
        /// Per symbol tarification
        /// </summary>
        /// <param name="cost">Maximum symbols for one request</param>
        /// <param name="queryParameter">Name of query parameter for counting symbols</param>
        /// <remarks>
        /// In PerSymbol mode cost means maximum symbols for one request.
        /// Example: If cost = 50 then 45 symbols = 1 request, 54 = 2 requests.
        /// Symbols count always rounded to upper bound.
        /// </remarks>
        public ThrottleThisAttribute(int cost, string queryParameter)
        {
            Mode = TarificationMode.PerSymbol;
            QueryParameter = queryParameter;
            Cost = cost;
        }

        /// <summary>
        /// Used for PerWord & PerSymbol tarification mode
        /// </summary>
        /// <param name="delimiter">symbol for delimiter</param>
        public ThrottleThisAttribute(int cost, char delimiter)
        {
            Cost = cost;
            Delimiter = delimiter;
            Mode = TarificationMode.PerWord;
        }

        public int Cost { get; }

        public char Delimiter { get; }

        public string QueryParameter { get; }

        public TarificationMode Mode { get; }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LogThisAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class OnlyForPaidAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class CheckForPayedAttribute : Attribute
    {
    }
}