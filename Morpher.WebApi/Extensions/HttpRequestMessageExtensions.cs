namespace Morpher.WebService.V3.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.ServiceModel.Channels;
    using System.Text;

    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Shared.Models.Exceptions;

    using Newtonsoft.Json;

    public static class HttpRequestMessageExtensions
    {
        private static readonly JsonMediaTypeFormatter JsonMediaTypeFormatter;

        private static readonly XmlMediaTypeFormatter XmlMediaTypeFormatter;

        static HttpRequestMessageExtensions()
        {
            JsonMediaTypeFormatter = new JsonMediaTypeFormatter { SerializerSettings = { Formatting = Formatting.Indented } };
            XmlMediaTypeFormatter = new XmlMediaTypeFormatter();
        }

        public static HttpResponseMessage CreateResponse<T>(
            this HttpRequestMessage message,
            HttpStatusCode statusCode,
            T value,
            ResponseFormat? format)
        {           
            switch (format)
            {
                case ResponseFormat.Json:
                    return message.CreateResponse(statusCode, value, JsonMediaTypeFormatter);
                case ResponseFormat.Xml:
                    return message.CreateResponse(statusCode, value, XmlMediaTypeFormatter);
                default:
                    return message.Headers.Accept.ToString() == "application/json" ? 
                        message.CreateResponse(statusCode, value, JsonMediaTypeFormatter) : 
                        message.CreateResponse(statusCode, value, XmlMediaTypeFormatter);
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

            throw new InvalidTokenFormat();
        }

        public static Guid? GetToken(this HttpRequestMessage message)
        {
            string token = message.GetQueryString("token") ?? message.GetBasicAuthorization();

            if (token == null)
            {
                return null;
            }

            Guid guid;
            if (Guid.TryParse(token, out guid))
            {
                return guid;
            }

            throw new InvalidTokenFormat();
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
            IEnumerable<string> keys;
            if (!message.Headers.TryGetValues(key, out keys))
            {
                return null;
            }

            return keys.First();
        }

        public static string GetClientIp(this HttpRequestMessage message)
        {
            if (message.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((dynamic)message.Properties["MS_HttpContext"]).Request.UserHostAddress;
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