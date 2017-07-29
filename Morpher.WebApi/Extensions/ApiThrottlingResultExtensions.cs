// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Extensions
{
    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Shared.Models.Exceptions;

    public static class ApiThrottlingResultExtensions
    {
        public static MorpherException GenerateMorpherException(this ApiThrottlingResult result)
        {
            switch (result)
            {
                case ApiThrottlingResult.TokenNotFound: return new TokenNotFoundException();
                case ApiThrottlingResult.InvalidToken: return new InvalidTokenFormat();
                case ApiThrottlingResult.IpBlocked: return new IpBlockedException();
                case ApiThrottlingResult.Overlimit: return new ExceededDailyLimitException();
                case ApiThrottlingResult.Unpaid: return new NotPayedException();
                default: return new MorpherException("unknown exception", 1);
            }
        }
    }
}