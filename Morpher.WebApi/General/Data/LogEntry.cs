namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public class LogEntry
    {
        public LogEntry(
            string remoteAddress,
            string queryString,
            string querySource,
            DateTime dateTime,
            Guid? webServiceToken,
            Guid? userId,
            string userAgent,
            int errorCode)
        {
            RemoteAddress = remoteAddress;
            QueryString = queryString;
            QuerySource = querySource;
            DateTimeUTC = dateTime;
            WebServiceToken = webServiceToken;
            UserId = userId;
            UserAgent = userAgent;
            ErrorCode = errorCode;
        }

        public string RemoteAddress { get; }

        public string QueryString { get; }

        public string QuerySource { get; }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public DateTime DateTimeUTC { get; }

        public Guid? UserId { get; }

        public Guid? WebServiceToken { get; }

        public string UserAgent { get; }

        public int ErrorCode { get; }
    }
}