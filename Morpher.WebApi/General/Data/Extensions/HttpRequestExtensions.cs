namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Text;
    using System.Web;

    public static class HttpRequestExtensions
    {
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