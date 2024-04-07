namespace Aralash.Domain.Models;

public class UserView
{
    public required string Id { get; init; }
    
    public required string Username { get; init; }
    
    public string? Lastname { get; init; }
    public string? Firstname { get; init; }
    public string? Patronymic { get; init; }
    
    public bool IsActive { get; set; }
}