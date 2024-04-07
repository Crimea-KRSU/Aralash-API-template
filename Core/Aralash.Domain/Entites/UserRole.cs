namespace Aralash.Domain.Entites;

[PrimaryKey(nameof(UserId), nameof(RoleId))]
public class UserRole : IBaseEntity
{
    public required string UserId { get; set; }
    public User User { get; set; }
    public required string RoleId { get; set; }
    public Role Role { get; set; }
}