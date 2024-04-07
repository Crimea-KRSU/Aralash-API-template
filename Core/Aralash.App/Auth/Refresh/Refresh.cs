namespace Aralash.App.Auth.Refresh;

public record RefreshCommand(string RefreshToken) : ICommand<TokenDto>;

public class RefreshCommandHandler : ICommandHandler<RefreshCommand, TokenDto>
{
    private readonly IAralashDbContext _uow;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITokenManager _tokenService;

    public RefreshCommandHandler(
        IAralashDbContext uow,
        IJwtProvider jwtProvider,
        ITokenManager tokenService)
    {
        _uow = uow;
        _jwtProvider = jwtProvider;
        _tokenService = tokenService;
    }
    
    public async Task<TokenDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var userClaims = _jwtProvider.ValidateRefreshToken(request.RefreshToken);
        await _tokenService.FindToken(request.RefreshToken, cancellationToken);

        var userId = userClaims.GetValueOrDefault(JwtClaims.UserId);
        if (userId == null)
            throw new AuthenticationException("Непредвиденная ошибка при обновлении токена. Авторизуйтесь заново");
        
        var user = await _uow.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        if (user is not { IsActive: true })
            throw new AuthenticationException("Ваша учетная запись не найдена в системе или не активирована. Обратитесь к администратору");
        
        var tokens = _jwtProvider.GenerateTokens(user);
        await _tokenService.SaveToken(user, tokens.RefreshToken, cancellationToken);
        return tokens;
    }
}