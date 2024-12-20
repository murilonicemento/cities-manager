using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class CustomControllerBase : ControllerBase
{
}