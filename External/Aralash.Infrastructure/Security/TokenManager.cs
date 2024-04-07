namespace Aralash.Infrastructure.Security;

public class TokenManager : ITokenManager
{
    private readonly IAralashDbContext _uow;

    public TokenManager(IAralashDbContext uow)
    {
        _uow = uow;
    }
    
    public async Task SaveToken(User user, string refreshToken, CancellationToken cancellationToken)
    {
        var newToken = new Token
        {
            UserId = user.Id,
            RefreshToken = refreshToken
        };
        await _uow.Tokens.AddAsync(newToken, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
    }

    public async Task FindToken(string refreshToken, CancellationToken cancellationToken)
    {
        var result = await _uow.Tokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken)
                     ?? throw new AuthenticationException("Refresh token истек или некорректен");
    }

    public async Task RemoveTokens(string userId, CancellationToken cancellationToken)
    {
        var tokens = await _uow.Tokens.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
        _uow.Tokens.RemoveRange(tokens);
        await _uow.SaveChangesAsync(cancellationToken);
    }
    
    public async Task RemoveToken(string refreshToken, CancellationToken cancellationToken)
    {
        var token = await _uow.Tokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);
        if (token != null)
        {
            _uow.Tokens.Remove(token);
            await _uow.SaveChangesAsync(cancellationToken);   
        }
    }
}