namespace Aralash.Domain.Entites;

public class User : IBaseEntity<string>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    [MaxLength(36)]
    public required string Id { get; set; }
    
    [MaxLength(32)]
    public required string Username { get; set; }
    
    public required string Password { get; set; }
    public required string PasswordSalt { get; set; }
    
    [MaxLength(50)]
    public string? Lastname { get; set; }
    [MaxLength(50)]
    public string? Firstname { get; set; }
    [MaxLength(50)]
    public string? Patronymic { get; set; }
    
    public bool IsActive { get; set; }
    
    public ICollection<UserRole>? Roles { get; set; }
}
