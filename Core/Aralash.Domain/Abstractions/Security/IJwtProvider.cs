namespace Aralash.Domain.Abstractions.Security;

public interface IJwtProvider
{
    public TokenDto GenerateTokens(User user);
    Dictionary<string,string> ValidateRefreshToken(string refreshToken);
    Dictionary<string,string> ValidateAccessToken(string accessToken);
}