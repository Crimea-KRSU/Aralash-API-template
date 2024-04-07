using AutoMapper;

namespace Aralash.App.Roles;

[SecuredOperation("Получение списка ролей")]
public record GetRolesQuery(string? NamePattern) : ListQuery<RoleView>;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, ListQueryResult<RoleView>>
{
    private readonly IAralashDbContext _uow;
    private IMapper _mapper;

    public GetRolesQueryHandler(IAralashDbContext uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }
    
    public async Task<ListQueryResult<RoleView>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var resQuery = _uow.Roles.AsNoTracking()
            .When(request.NamePattern.IsNotEmpty(),
            q => q.Where(x => EF.Functions.Like(x.Name, $"{request.NamePattern}%")));
        var count = await resQuery.CountAsync(cancellationToken);
        var result = await _mapper.ProjectTo<RoleView>(resQuery
                .OrderBy(x => x.Name)
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage))
            .ToListAsync(cancellationToken);
        return new ListQueryResult<RoleView>(result, count);
    }
}