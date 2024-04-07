using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Aralash.Infrastructure.Security;

public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
{
    public TokenDto GenerateTokens(User user)
    {
        Claim[] claims =
        [
            new Claim(JwtClaims.UserId, user.Id),
            new Claim(JwtClaims.IsActivated, user.IsActive.ToString())
        ];

        var accessSigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.JwtAccessSecretKey)),
            SecurityAlgorithms.HmacSha256);

        var refreshSigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.JwtRefreshSecretKey)),
            SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: accessSigningCredentials,
            expires: DateTime.UtcNow.AddMinutes(jwtOptions.Value.JwtAccessExpiredMinutes));

        var refreshToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: refreshSigningCredentials,
            expires: DateTime.UtcNow.AddDays(jwtOptions.Value.JwtRefreshExpiredDays));

        var accessTokenValue = new JwtSecurityTokenHandler().WriteToken(accessToken);
        var refreshTokenValue = new JwtSecurityTokenHandler().WriteToken(refreshToken);

        return new TokenDto(accessTokenValue, refreshTokenValue);
    }

    public Dictionary<string,string> ValidateRefreshToken(string refreshToken) =>
        ValidateToken(refreshToken, jwtOptions.Value.JwtRefreshSecretKey);

    public Dictionary<string,string> ValidateAccessToken(string accessToken) =>
        ValidateToken(accessToken, jwtOptions.Value.JwtAccessSecretKey);

    private Dictionary<string,string> ValidateToken(string token, string secretKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
            ValidateIssuer = false, 
            ValidateAudience = false, 
            RequireExpirationTime = true,
            ValidateLifetime = true 
        };

        try
        { 
            tokenHandler.ValidateToken(token, validationParameters, out _);

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            return jwtSecurityToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
        }
        catch (SecurityTokenExpiredException)
        {
            throw;
        }
        catch (SecurityTokenException)
        {
            throw new AuthenticationException();
        }
    }
}