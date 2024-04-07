using Aralash.App.Abstractions.Query;
using AutoMapper;

namespace Aralash.App.User.GetUserInfo;

public record GetUsersCommand(string? NamePattern, string? UsernamePattern) : ListQuery<UserView>;

public class GetUsersCommandHandler : IRequestHandler<GetUsersCommand, ListQueryResult<UserView>>
{
    private readonly IAralashDbContext _uow;
    private readonly IMapper _mapper;

    public GetUsersCommandHandler(IAralashDbContext uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }
    
    public async Task<ListQueryResult<UserView>> Handle(GetUsersCommand request, CancellationToken cancellationToken)
    {
        var query = _uow.Users.AsNoTracking()
            .When(request.NamePattern.IsNotEmpty(),
                x => x.Where(u =>
                    EF.Functions.Like(u.Firstname, $"{request.NamePattern}%") ||
                    EF.Functions.Like(u.Lastname, $"{request.NamePattern}%") ||
                    EF.Functions.Like(u.Patronymic, $"{request.NamePattern}%")))
            .When(request.UsernamePattern.IsNotEmpty(),
                x => x.Where(u => EF.Functions.Like(u.Username, $"{request.UsernamePattern}%")));
        var count = await query.CountAsync(cancellationToken);
        query = query.Skip((request.Page - 1) * request.PerPage).Take(request.PerPage);
        var result = await _mapper.ProjectTo<UserView>(query).ToListAsync(cancellationToken);
        return new ListQueryResult<UserView>(result, count);
    }
}