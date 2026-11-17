using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
///     Routes for monitoring system health. Useful for Docker health checks.
/// </summary>
[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    /// <summary>
    ///     Gets a list of all internal services and their statuses.
    /// </summary>
    [HttpGet]
    public IActionResult Check()
    {
        var currentHealth = HealthService.CurrentHealth;
        var healthy = currentHealth.All(pair => pair.Value);

        if (healthy) return Ok(currentHealth);

        return new ObjectResult(currentHealth)
        {
            StatusCode = 503
        };
    }
}