using AutoMapper;

namespace Aralash.App.User.GetUserInfo;

public record GetUserInfoCommand(string UserId) : IRequest<UserView>;

public class GetUserInfoCommandHandler : IRequestHandler<GetUserInfoCommand, UserView>
{
    private readonly IAralashDbContext _uow;
    private readonly IMapper _mapper;

    public GetUserInfoCommandHandler(IAralashDbContext uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }
    
    public async Task<UserView> Handle(GetUserInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user == null)
        {
            throw new ArgumentException("Указанный пользователь не найден");
        }

        return _mapper.Map<UserView>(user);
    }
}