namespace Aralash.Domain.Options;

public class JwtOptions
{
    public string JwtAccessSecretKey { get; init; }
    public string JwtRefreshSecretKey { get; init; }
    public int JwtRefreshExpiredDays { get; init; }
    public int JwtAccessExpiredMinutes { get; set; }
}