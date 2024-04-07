namespace Aralash.API.Services;

public class CurrentUser : ICurrentUser
{
    public string? UserId { get; }

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext == null)
        {
            UserId = null;
            return;
        }
        UserId = httpContextAccessor.HttpContext.User.FindFirstValue(JwtClaims.UserId);
    }
}