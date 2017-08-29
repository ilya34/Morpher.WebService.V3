namespace Morpher.WebService.V3.General.Data
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