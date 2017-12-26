namespace Morpher.WebService.V3.General
{
    using System;
    using System.Text;
    using System.Web;

    public static class HttpRequestExtensions
    {
        public static ResponseFormat GetResponseFormat(this HttpRequest request)
        {
            var requestedResponseFormat =
                request.QueryString.Get("format") ?? request.Headers.Get("Accept");
            ResponseFormat responseFormat = ResponseFormat.Xml;
            if (requestedResponseFormat != null
                && (requestedResponseFormat.ToLowerInvariant() == "json"
                    || requestedResponseFormat.Contains("application/json")))
                responseFormat = ResponseFormat.Json;
            return responseFormat;
        }

        public static Guid? GetToken(this HttpRequest request)
        {
            string token = request.QueryString.Get("token") ?? GetBasicAuthorization(request);

            if (token == null)
            {
                return null;
            }

            Guid guid;
            if (Guid.TryParse(token, out guid))
            {
                return guid;
            }

            throw new InvalidTokenFormatException();
        }

        public static string GetBasicAuthorization(this HttpRequest request)
        {
            string auth = request.Headers.Get("Authorization");
            if (auth == null)
            {
                return null;
            }

            if (auth.StartsWith("Basic"))
            {
                string token = auth.Substring("Basic".Length).Trim();
                return Encoding.UTF8.GetString(Convert.FromBase64String(token));
            }

            throw new InvalidTokenFormatException();
        }
    }
}