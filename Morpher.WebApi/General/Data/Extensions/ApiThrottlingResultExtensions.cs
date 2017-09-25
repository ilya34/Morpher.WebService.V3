namespace Morpher.WebService.V3.General.Data
{
    public static class ApiThrottlingResultExtensions
    {
        public static MorpherException GenerateMorpherException(this ApiThrottlingResult result)
        {
            switch (result)
            {
                case ApiThrottlingResult.TokenNotFound: return new TokenNotFoundExceptionException();
                case ApiThrottlingResult.InvalidToken: return new InvalidTokenFormatException();
                case ApiThrottlingResult.IpBlocked: return new IpBlockedExceptionException();
                case ApiThrottlingResult.Overlimit: return new ExceededDailyLimitException();
                case ApiThrottlingResult.Unpaid: return new NotPayedException();
                default: return new MorpherException("unknown exception", 1);
            }
        }
    }
}