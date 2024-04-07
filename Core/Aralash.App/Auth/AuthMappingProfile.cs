using AutoMapper;

namespace Aralash.App.Auth;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<Domain.Entites.User, UserView>();
        CreateMap<Role, RoleView>();
        CreateMap<SecuredOperation, SecuredOperationView>();
    }
}