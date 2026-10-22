using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Check()
    {
        var currentHealth = HealthService.CurrentHealth;
        var healthy = currentHealth.All(pair => pair.Value != false);

        if (healthy)
        {
            return Ok(currentHealth);
        }
        else
        {
            return new ObjectResult(currentHealth)
            {
                StatusCode = 503
            };
        }
    }
}