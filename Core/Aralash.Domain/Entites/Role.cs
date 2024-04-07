namespace Aralash.Domain.Entites;

public class Role : IBaseEntity<string>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    [MaxLength(36)]
    public required string Id { get; set; }
    
    [MaxLength(50)]
    public required string Name { get; set; }
    
    [MaxLength(100)]
    public string? Description { get; set; }
    
    public ICollection<RoleOperation> Operations { get; set; }
}