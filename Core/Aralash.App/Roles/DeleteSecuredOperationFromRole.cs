namespace Aralash.App.Roles;

[SecuredOperation("Запрет на использование защищенной операции ролью")]
public record DeleteSecuredOperationFromRoleCommand(string RoleId, string SecuredOperationId) : ICommand;

public class DeleteSecuredOperationFromRoleHandler : ICommandHandler<DeleteSecuredOperationFromRoleCommand>
{
    private readonly IAralashDbContext _uow;

    public DeleteSecuredOperationFromRoleHandler(IAralashDbContext uow)
    {
        _uow = uow;
    }
    
    public async Task Handle(DeleteSecuredOperationFromRoleCommand request, CancellationToken cancellationToken)
    {
        var roleOpExist = await _uow.RoleOperations
            .FirstOrDefaultAsync(x => x.RoleId == request.RoleId 
                                      && x.OperationId == request.SecuredOperationId,
                cancellationToken);
        if (roleOpExist is null)
            throw new ArgumentException("Роль не имеет заданную защищенную операцию");
        _uow.RoleOperations.Remove(roleOpExist);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}