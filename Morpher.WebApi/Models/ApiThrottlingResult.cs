namespace Morpher.WebApi.ApiThrottler
{
    public enum ApiThrottlingResult
    {
        TokenNotFound,
        InvalidToken,
        Overlimit,
        Success,
        Unpaid,
        IpBlocked
    }
}