using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace API.Models;

/// <summary>
///     The structure of how users are stored in our persistent database.
/// </summary>
public class UserModel : IdentityUser<Guid>
{
    /// <summary>
    ///     The servers the user owns.
    /// </summary>
    public ICollection<ServerModel> Servers { get; set; } = [];

    /// <summary>
    ///     An authorization code used to perform critical actions by the owner on the account.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public UserAuthorizationCode? AuthorizationCode { get; set; }
}

/// <summary>
///     A role type for a user.
/// </summary>
public enum UserRole
{
    Client,
    Admin,
    SuperAdmin
}

/// <summary>
///     An authorization code for a sensitive action under a user's account.
/// </summary>
/// <param name="Code">The code.</param>
/// <param name="GeneratedAt">The time the code was generated at.</param>
public record UserAuthorizationCode([MaxLength(512)] string Code, DateTime GeneratedAt);