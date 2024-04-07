using AutoMapper;

namespace Aralash.App.Roles;

[SecuredOperation("Создание роли")]
public record CreateRoleCommand(string? Description, string Name) : ICommand<string>;

public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, string>
{
    private readonly IAralashDbContext _uow;
    private readonly IMapper _mapper;

    public CreateRoleCommandHandler(IAralashDbContext uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }
    
    public async Task<string> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var roleExist = await _uow.Roles.AnyAsync( x => x.Name == request.Name, cancellationToken);
        if (roleExist)
            throw new ArgumentException("Роль с таким названием уже существует", nameof(request.Name));

        var dbEntity = _mapper.Map<Role>(request);
        dbEntity.Id = Guid.NewGuid().ToString();
        await _uow.Roles.AddAsync(dbEntity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return dbEntity.Id;
    }
}