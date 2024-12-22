using System.Globalization;
using System.Security.Claims;
using CitiesManager.WebAPI.DTO;
using CitiesManager.WebAPI.Identity;
using CitiesManager.WebAPI.ServicesContracts;
using System.IdentityModel.Tokens.Jwt;
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
            new(ClaimTypes.Name, user.PersonName ?? string.Empty)
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
            Expiration = expiration
        };
    }
}