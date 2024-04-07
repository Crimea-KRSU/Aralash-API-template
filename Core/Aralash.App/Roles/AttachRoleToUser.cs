namespace Aralash.App.Roles;

[SecuredOperation("Назначение роли пользователю")]
public record AttachRoleToUserCommand(string UserId, string RoleId) : ICommand;

public class AttachRoleToUserCommandHandler : ICommandHandler<AttachRoleToUserCommand>
{
    private readonly IAralashDbContext _uow;

    public AttachRoleToUserCommandHandler(IAralashDbContext uow)
    {
        _uow = uow;
    }
    
    public async Task Handle(AttachRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var roleNotExist = await _uow.Roles.FirstOrDefaultAsync(x => x.Id == request.RoleId, cancellationToken) == null;
        if(roleNotExist)
            throw new ArgumentException("Роль не найдена");
        
        var userNotExist = await _uow.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken) == null;
        if(roleNotExist)
            throw new ArgumentException("Пользователь не найден");
        
        var userHasRole =
            await _uow.UserRoles
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.RoleId == request.RoleId,
                cancellationToken);
        
        if (userHasRole is not null)
            throw new ArgumentException("У пользователя уже есть данная роль");
        
        var entity = new UserRole()
        {
            RoleId = request.RoleId,
            UserId = request.UserId
        };
        await _uow.UserRoles.AddAsync(entity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}