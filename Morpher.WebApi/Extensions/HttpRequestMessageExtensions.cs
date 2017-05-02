namespace Morpher.WebApi.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Web;

    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Exceptions;

    public static class HttpRequestMessageExtensions
    {
        public static HttpResponseMessage CreateResponse<T>(
            this HttpRequestMessage message,
            HttpStatusCode statusCode,
            T value,
            ResponseFormat? format)
        {
            switch (format)
            {
                case ResponseFormat.Json:
                    return message.CreateResponse(statusCode, value, new JsonMediaTypeFormatter());
                case ResponseFormat.Xml:
                    return message.CreateResponse(statusCode, value, new XmlMediaTypeFormatter());
                default:
                    return message.CreateResponse(statusCode, value);
            }
        }

        public static string GetBasicAuthorization(this HttpRequestMessage message)
        {
            string auth = message.GetHeader("Authorization");


            if (auth == null)
            {
                return null;
            }

            if (auth.StartsWith("Basic"))
            {
                string token = auth.Substring("Basic".Length).Trim();
                return Encoding.UTF8.GetString(Convert.FromBase64String(token));
            }

            return "Invalid token";
        }

        public static Guid? GetToken(this HttpRequestMessage message)
        {

            if (Guid.TryParse(message.GetQueryString("token") ?? message.GetBasicAuthorization(), out Guid guid))
            {
                return guid;
            }

            return null;
        }

        public static Dictionary<string, string> GetQueryStrings(this HttpRequestMessage message)
        {
            return message.GetQueryNameValuePairs()
                .ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.OrdinalIgnoreCase);
        }

        public static string GetQueryString(this HttpRequestMessage message, string key)
        {
            IEnumerable<KeyValuePair<string, string>> queryStrings = message.GetQueryNameValuePairs();
            if (queryStrings == null)
            {
                return null;
            }

            KeyValuePair<string, string> match = queryStrings.FirstOrDefault(pair => string.Compare(pair.Key, key, StringComparison.OrdinalIgnoreCase) == 0);
            return string.IsNullOrEmpty(match.Value) ? null : match.Value;
        }

        public static string GetHeader(this HttpRequestMessage message, string key)
        {
            if (!message.Headers.TryGetValues(key, out IEnumerable<string> keys))
            {
                return null;
            }

            return keys.First();
        }

        public static string GetClientIp(this HttpRequestMessage message)
        {
            if (message.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)message.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }

            if (message.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                var prop = (RemoteEndpointMessageProperty)message.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }

            return null;
        }
    }
}