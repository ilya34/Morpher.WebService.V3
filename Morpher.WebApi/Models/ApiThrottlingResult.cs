namespace Morpher.WebApi.ApiThrottler
{
    public enum ApiThrottlingResult
    {
        InvalidToken,
        Overlimit,
        Success,
        Unpaid,
        IpBlocked
    }
}