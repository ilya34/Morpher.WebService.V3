namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Microsoft.Owin;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class IOwinRequestExtensions
    {
        private static string GetBasicAuthorization(this IOwinRequest request)
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

        public static Guid? GetToken(this IOwinRequest request)
        {
            string token = request.Query.Get("token") ?? request.GetBasicAuthorization();
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
    }
}