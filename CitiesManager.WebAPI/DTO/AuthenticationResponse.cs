namespace CitiesManager.WebAPI.DTO;

public class AuthenticationResponse
{
    public string? PersonName { get; set; } = string.Empty;

    public string? Email { get; set; } = string.Empty;

    public string? Token { get; set; } = string.Empty;

    public DateTime? Expiration { get; set; }

    public string? RefreshToken { get; set; } = string.Empty;

    public DateTime RefreshTokenExpirationDate { get; set; }
}