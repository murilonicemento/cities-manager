using System.Globalization;
using System.Security.Claims;
using CitiesManager.WebAPI.DTO;
using CitiesManager.WebAPI.Identity;
using CitiesManager.WebAPI.ServicesContracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CitiesManager.WebAPI.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthenticationResponse CreateJwt(ApplicationUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));
        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new(ClaimTypes.NameIdentifier, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.PersonName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        ];

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken tokenGenerator = new(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials
        );
        JwtSecurityTokenHandler tokenHandler = new();

        var token = tokenHandler.WriteToken(tokenGenerator);

        return new AuthenticationResponse
        {
            Token = token,
            Email = user.Email,
            PersonName = user.PersonName,
            Expiration = expiration,
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExpirationDate =
                DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["RefreshToken:EXPIRATION_DATE"]))
        };
    }

    public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = _configuration["Jwt:Audience"],
            ValidateIssuer = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty)),
            ValidateLifetime = false
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var principal =
            jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid Token");
        }

        return principal;
    }

    private static string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        var randomNumberGenerator = RandomNumberGenerator.Create();

        randomNumberGenerator.GetBytes(bytes);

        return Convert.ToBase64String(bytes);
    }
}