namespace Aralash.App.Roles;

[SecuredOperation("Удаление роли")]
public record DeleteRoleCommand(string RoleId) : ICommand;

public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand>
{
    private readonly IAralashDbContext _uow;

    public DeleteRoleCommandHandler(IAralashDbContext uow)
    {
        _uow = uow;
    }
    
    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var roleExist = await _uow.Roles.FirstOrDefaultAsync(x => x.Id == request.RoleId, cancellationToken);
        if (roleExist is null)
            throw new ArgumentException("Роль не существует", nameof(request.RoleId));
        
        _uow.Roles.Remove(roleExist);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}