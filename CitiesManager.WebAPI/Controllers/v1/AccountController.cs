using CitiesManager.WebAPI.DTO;
using CitiesManager.WebAPI.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

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

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpPost]
    public async Task<ActionResult<ApplicationUser>> Post(RegisterDTO registerDto)
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

            return Ok(user);
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
}