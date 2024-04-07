namespace Aralash.App.Roles;

[SecuredOperation("Добавление права на защищенную операцию в роль")]
public record AddSecuredOperationToRoleCommand(string RoleId, string SecuredOperationId) : ICommand;

public class AddSecuredOperationToRoleCommandHandler : ICommandHandler<AddSecuredOperationToRoleCommand>
{
    private readonly IAralashDbContext _uow;

    public AddSecuredOperationToRoleCommandHandler(IAralashDbContext uow)
    {
        _uow = uow;
    }
    
    public async Task Handle(AddSecuredOperationToRoleCommand request, CancellationToken cancellationToken)
    {
        var soExist =
            await _uow.SecuredOperations
                .FirstOrDefaultAsync(x => x.Id == request.SecuredOperationId,
                cancellationToken);
        if (soExist is null)
            throw new ArgumentException("Защищенная операция не найдена", nameof(request.SecuredOperationId));

        var roleExist = await _uow.Roles.FirstOrDefaultAsync(x => x.Id == request.RoleId, cancellationToken);
        if (roleExist is null)
            throw new ArgumentException("Роль не найдена", nameof(request.RoleId));

        var roleOpExist = await _uow.RoleOperations
            .FirstOrDefaultAsync(x => x.RoleId == request.RoleId && x.OperationId == request.SecuredOperationId,
                cancellationToken);
        if (roleOpExist is not null)
            throw new ArgumentException("Роль уже имеет заданную защищенную операцию");
        var dbEntity = new RoleOperation()
        {
            OperationId = request.SecuredOperationId,
            RoleId = request.RoleId
        };
        await _uow.RoleOperations.AddAsync(dbEntity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}