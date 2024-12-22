using CitiesManager.WebAPI.DTO;
using CitiesManager.WebAPI.Identity;
using CitiesManager.WebAPI.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers.v1;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
[ApiVersion("1.0")]
public class AccountController : CustomControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IJwtService _jwtService;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
    }

    [HttpPost]
    public async Task<ActionResult<ApplicationUser>> Register(RegisterDTO registerDto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessage = string.Join(" | ",
                ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage));

            return Problem(errorMessage, statusCode: 400);
        }

        var user = new ApplicationUser
        {
            PersonName = registerDto.PersonName,
            UserName = registerDto.Email,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);

            var authenticationResponse = _jwtService.CreateJwt(user);

            return Ok(authenticationResponse);
        }

        var errors = string.Join(" | ", result.Errors.Select(error => error.Description));

        return Problem(errors);
    }

    [HttpGet]
    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return Ok(user is null);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO loginDto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(value => value.Errors)
                .Select(error => error.ErrorMessage));

            return Problem(errorMessage, statusCode: 400);
        }

        var result = await _signInManager.PasswordSignInAsync(userName: loginDto.Email, password: loginDto.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        if (!result.Succeeded) return Problem("Invalid email or password", statusCode: 400);

        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is null) return NoContent();

        var authenticationResponse = _jwtService.CreateJwt(user);

        return Ok(authenticationResponse);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return NoContent();
    }
}