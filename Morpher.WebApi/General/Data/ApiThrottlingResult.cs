namespace Morpher.WebService.V3.General
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