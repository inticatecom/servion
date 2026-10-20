using API.DTO;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public sealed class UserController(UserService users, UserManager<UserModel> userManager) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserDto.Create request)
    {
        var user = await users.CreateUserAsync(request.Username, request.Email, request.Password);
        return Ok(user.Id);
    }

    [HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserDto.Authenticate request)
    {
        var success = await users.AuthenticateUserAsync(request.Email, request.Password);
        if (!success)
        {
            return Unauthorized();
        }

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Me()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            user.Id,
            user.UserName,
            user.Email
        });
    }
}