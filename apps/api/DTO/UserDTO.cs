using System.ComponentModel.DataAnnotations;

namespace API.DTO;

/// <summary>
/// The user DTO for request relating to user operations.
/// </summary>
public static class UserDto
{
    /// <summary>
    /// A create operation attempting to create a new user.
    /// </summary>
    /// <param name="Username">The username.</param>
    /// <param name="Email">The email address.</param>
    /// <param name="Password">The password (will be hashed when stored).</param>
    public record Create([Required] string Username, [Required] string Email, [Required] string Password);

    public record Authenticate([Required] string Email, [Required] string Password);
}