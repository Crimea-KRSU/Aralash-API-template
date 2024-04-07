using AutoMapper;

namespace Aralash.App.Roles;

public class RolesMappingProfile : Profile
{
    public RolesMappingProfile()
    {
        CreateMap<Role, CreateRoleCommand>();
        CreateMap<CreateRoleCommand, Role>();
        CreateMap<Role, RoleView>();
        CreateMap<SecuredOperation, SecuredOperationView>();
    }
}