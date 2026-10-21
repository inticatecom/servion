using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController(HealthService health) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Check()
    {
        Dictionary<string, Func<Task<bool>>> checks = new()
        {
            ["database"] = health.CheckDatabaseHealth
        };
        Dictionary<string, bool> results = new();

        foreach (var (name, check) in checks)
        {
            results[name] = await check();
        }

        var healthy = results.All(pair => pair.Value != false);
        if (healthy)
        {
            return Ok(results);
        }
        else
        {
            return new ObjectResult(results)
            {
                StatusCode = 503
            };
        }
    }
}