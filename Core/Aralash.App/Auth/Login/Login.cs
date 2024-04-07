namespace Aralash.App.Auth.Login;

public record LoginCommand(string Username, string Password) : ICommand<TokenDto>;

public class LoginCommandHandler : ICommandHandler<LoginCommand, TokenDto>
{
    private readonly IAralashDbContext _uow;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITokenManager _tokenManager;

    public LoginCommandHandler(IAralashDbContext unitOfWork, IJwtProvider jwtProvider, ITokenManager tokenManager)
    {
        _uow = unitOfWork;
        _jwtProvider = jwtProvider;
        _tokenManager = tokenManager;
    }
    
    public async Task<TokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users
            .FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

        if (user == null)
            throw new AuthenticationException("Пользователя с таким логином не существует");

        var passwordCorrect = Sha256Helper.Verify(request.Password, user.Password, user.PasswordSalt);
        if (!passwordCorrect)
            throw new AuthenticationException("Неправильный логин или пароль");
        
        if (!user.IsActive)
            throw new UnauthorizedAccessException("Учетная запись не активирована. Обратитесь к администратору");

        var tokens = _jwtProvider.GenerateTokens(user);
        await _tokenManager.SaveToken(user, tokens.RefreshToken, cancellationToken);

        return tokens;
    }
}