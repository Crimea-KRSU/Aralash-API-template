namespace Aralash.Domain.Entites;

public class Token : IBaseEntity
{
    public string RefreshToken { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
}