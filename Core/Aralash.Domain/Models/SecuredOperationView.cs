namespace Aralash.Domain.Models;

public class SecuredOperationView
{
    public required string Id { get; set; }
    
    public required string OperationName { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime CreateDate { get; set; }
}