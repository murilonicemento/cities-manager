using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
// [Route("api/[controller]")]
public class CustomControllerBase : ControllerBase
{
}