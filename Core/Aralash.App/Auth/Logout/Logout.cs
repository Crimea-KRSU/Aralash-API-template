namespace Aralash.App.Auth.Logout;

public record LogoutCommand(string RefreshToken) : ICommand;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly ITokenManager _manager;

    public LogoutCommandHandler(ITokenManager manager)
    {
        _manager = manager;
    }
    
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _manager.RemoveToken(request.RefreshToken, cancellationToken);
    }
}