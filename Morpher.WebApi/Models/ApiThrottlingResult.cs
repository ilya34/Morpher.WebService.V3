namespace Morpher.WebService.V3.Models
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