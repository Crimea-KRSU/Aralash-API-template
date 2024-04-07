namespace Aralash.Domain.Entites;

public class SecuredOperation : IBaseEntity<string>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    [MaxLength(36)]
    public required string Id { get; set; }
    
    [MaxLength(125)]
    public required string OperationName { get; set; }
    
    [MaxLength(200)]
    public string? Description { get; set; }
    
    public DateTime CreateDate { get; set; }
    
    public ICollection<RoleOperation>? Roles { get; set; }
}