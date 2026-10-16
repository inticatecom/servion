using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

/// <summary>
/// The structure of how users are stored in our persistent database.
/// </summary>
[Index(nameof(Username), IsUnique = true)]
public class UserModel : IdentityModel
{
    /// <summary>
    /// The username of the user.
    /// </summary>
    [StringLength(30, MinimumLength = 3)]
    public required string Username { get; set; }

    /// <summary>
    /// The email bound to the user's account.
    /// </summary>
    [StringLength(30, MinimumLength = 3)]
    public required string Email { get; set; }

    /// <summary>
    /// The password hash for the user. Must be validated with the hasher via user service.
    /// </summary>
    [MaxLength(512)]
    public required string PasswordHash { get; set; }

    /// <summary>
    /// The servers the user owns.
    /// </summary>
    public ICollection<ServerModel> Servers { get; set; } = new List<ServerModel>();

    /// <summary>
    /// An authorization code used to perform critical actions by the owner on the account.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public UserAuthorizationCode? AuthorizationCode { get; set; }

    /// <summary>
    /// The user's role, used for permission based operations.
    /// </summary>
    public UserRole Role { get; set; } = UserRole.Client;
}

public enum UserRole
{
    Client,
    Admin,
    SuperAdmin
}

public record UserAuthorizationCode([MaxLength(512)] string Code, DateTime GeneratedAt);