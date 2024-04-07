namespace Aralash.App.Auth.Register;

public record RegisterCommand(
    string Username,
    string Password,
    string Lastname,
    string Firstname,
    string Patronymic) : ICommand<TokenDto>;

public class RegistarCommandHandler : ICommandHandler<RegisterCommand, TokenDto>
{
    private readonly IAralashDbContext _uow;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITokenManager _tokenManager;

    public RegistarCommandHandler(IAralashDbContext unitOfWork, IJwtProvider jwtProvider, 
        ITokenManager tokenManager)
    {
        _uow = unitOfWork;
        _jwtProvider = jwtProvider;
        _tokenManager = tokenManager;
    }
    
    public async Task<TokenDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userEntity = await _uow.Users.FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

        if (userEntity != null)
            throw new ArgumentException("Пользователь с таким логином уже существует");

        var hashPlusSalt = Sha256Helper.HashStringWithSalt(request.Password);
        var newEntity = new Domain.Entites.User
        {
            Id = Guid.NewGuid().ToString(),
            Username = request.Username,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Patronymic = request.Patronymic,
            Password = hashPlusSalt.Hash,
            PasswordSalt = hashPlusSalt.Salt,
            IsActive = true // TODO email activation
        };

        await _uow.Users.AddAsync(newEntity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var tokens = _jwtProvider.GenerateTokens(newEntity);
        await _tokenManager.SaveToken(newEntity, tokens.RefreshToken, cancellationToken);
        return tokens;
    }
}