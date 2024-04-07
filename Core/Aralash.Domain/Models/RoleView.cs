namespace Aralash.Domain.Models;

public class RoleView
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? RoleId { get; set; }
}