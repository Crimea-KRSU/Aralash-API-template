using AutoMapper;

namespace Aralash.App.Roles;

[SecuredOperation("Получение списка защищаемых операций")]
public record GetSecuredOperationsQuery(string? Pattern) : ListQuery<SecuredOperationView>;

public class GetSecuredOperationsQueryHandler : IRequestHandler<GetSecuredOperationsQuery, ListQueryResult<SecuredOperationView>>
{
    private readonly IAralashDbContext _uow;
    private IMapper _mapper;

    public GetSecuredOperationsQueryHandler(IAralashDbContext uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }
    
    public async Task<ListQueryResult<SecuredOperationView>> Handle(GetSecuredOperationsQuery request, 
        CancellationToken cancellationToken)
    {
        var resQuery = _uow.SecuredOperations.AsNoTracking().When(request.Pattern.IsNotEmpty(),
            q => q.Where(x => 
                EF.Functions.Like(x.OperationName, $"{request.Pattern}%")));
        var count = await resQuery.CountAsync(cancellationToken);
        var result = await _mapper.ProjectTo<SecuredOperationView>(resQuery
                .OrderBy(x => x.OperationName)
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage))
            .ToListAsync(cancellationToken);
        return new ListQueryResult<SecuredOperationView>(result, count);
    }
}