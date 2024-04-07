namespace Aralash.Domain.Abstractions.Security;

public interface ICurrentUser
{
    public string? UserId { get; }
}