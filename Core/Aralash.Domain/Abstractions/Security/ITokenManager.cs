namespace Aralash.Domain.Abstractions.Security;

public interface ITokenManager
{
    Task SaveToken(User user, string refreshToken, CancellationToken cancellationToken);
    Task FindToken(string refreshToken, CancellationToken cancellationToken);
    Task RemoveToken(string refreshToken, CancellationToken cancellationToken);
    Task RemoveTokens(string userId, CancellationToken cancellationToken);
}