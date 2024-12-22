using CitiesManager.WebAPI.DTO;
using CitiesManager.WebAPI.Identity;

namespace CitiesManager.WebAPI.ServicesContracts;

public interface IJwtService
{
    AuthenticationResponse CreateJwt(ApplicationUser user);
}