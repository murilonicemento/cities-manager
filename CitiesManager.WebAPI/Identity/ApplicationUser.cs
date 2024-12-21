using Microsoft.AspNetCore.Identity;

namespace CitiesManager.WebAPI.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? PersonName { get; set; }
}