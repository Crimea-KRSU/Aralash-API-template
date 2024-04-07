namespace Aralash.Domain.Entites;

[PrimaryKey(nameof(RoleId), nameof(OperationId))]
public class RoleOperation : IBaseEntity
{
    public required string RoleId { get; set; }
    public Role Role { get; set; }
    public required string OperationId { get; set; }
    public SecuredOperation Operation { get; set; }
}