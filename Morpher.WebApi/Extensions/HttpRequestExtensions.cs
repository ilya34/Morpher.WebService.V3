namespace Morpher.WebService.V3.Extensions
{
    using System;
    using System.Web;
    using Models.Exceptions;

    public static class HttpRequestExtensions
    {
        public static Guid? GetToken(this HttpRequest request)
        {
            // TODO: Token from header
            string token = request.QueryString.Get("token");

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
    }
}