using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new {
            message = "API activa",
            endpoints = new[] { "/api/v1/producto", "/weatherforecast" }
        });
    }
}
