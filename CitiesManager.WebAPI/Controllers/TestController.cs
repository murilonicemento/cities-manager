using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public string Method() => "Hello World";
}