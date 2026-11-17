using API.DTO;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
///     Routes relating to the functionality of system users.
/// </summary>
[ApiController]
[Route("api/users")]
[Authorize]
public sealed class UserController(UserService users, UserManager<UserModel> userManager) : ControllerBase
{
    /// <summary>
    ///     Registers a new user into the system.
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserDto.Create request)
    {
        var user = await users.CreateUserAsync(request.Username, request.Email, request.Password);
        return Ok(user.Id);
    }


    /// <summary>
    ///     Authenticated if possible with the provided credentials.
    /// </summary>
    [HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserDto.Authenticate request)
    {
        return await users.AuthenticateUserAsync(request.Email, request.Password) ? Empty : Unauthorized();
    }

    /// <summary>
    ///     If properly authenticated, provide the user's information.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Me()
    {
        var user = await userManager.GetUserAsync(User);
        return user is not null
            ? Ok(new
            {
                user.Id,
                user.UserName,
                user.Email
            })
            : Unauthorized();
    }
}