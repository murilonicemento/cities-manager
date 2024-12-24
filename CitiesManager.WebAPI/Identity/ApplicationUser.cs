using Microsoft.AspNetCore.Identity;

namespace CitiesManager.WebAPI.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? PersonName { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiration { get; set; }
}