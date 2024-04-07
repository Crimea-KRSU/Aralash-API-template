namespace Aralash.App.Roles;

[SecuredOperation("Удалить роль у пользователя")]
public record DetachRoleFromUserCommand(string UserId, string RoleId) : ICommand;

public class DetachRoleFromUserCommandHandler : ICommandHandler<DetachRoleFromUserCommand>
{
    private readonly IAralashDbContext _uow;

    public DetachRoleFromUserCommandHandler(IAralashDbContext uow)
    {
        _uow = uow;
    }
    
    public async Task Handle(DetachRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        var inRole =
            await _uow.UserRoles
                .FirstOrDefaultAsync(x => x.RoleId == request.RoleId && x.UserId == request.UserId,
                cancellationToken);
        if (inRole == null)
            throw new ArgumentException("Пользователь не имеет данную роль");
        _uow.UserRoles.Remove(inRole);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}