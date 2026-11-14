using System.Security.Cryptography;
using System.Text;
using API.Contexts;
using API.Exceptions;
using API.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace API.Services;

/// <summary>
///     A service for managing users in the database.
/// </summary>
[UsedImplicitly]
public sealed class UserService(
    DatabaseContext database,
    UserManager<UserModel> userManager,
    SignInManager<UserModel> signInManager
)
{
    /// <summary>
    ///     Creates a new user with the provided information.
    /// </summary>
    /// <param name="username">The username for the user.</param>
    /// <param name="email">The email for the user.</param>
    /// <param name="password">The password to be hashed for the user.</param>
    /// <returns>The newly created user.</returns>
    public async Task<UserModel> CreateUserAsync(string username, string email, string password)
    {
        var user = new UserModel
        {
            UserName = username,
            Email = email
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(error => error.Description)
            );

            throw new UserException(400, errors);
        }

        var roleResult = await userManager.AddToRoleAsync(user, nameof(UserRole.Client));
        if (!roleResult.Succeeded)
            throw new UserException(
                500,
                "Failed to assign default user role."
            );

        return user;
    }

    public async Task<bool> AuthenticateUserAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return false;

        signInManager.AuthenticationScheme = IdentityConstants.ApplicationScheme;
        var result = await signInManager.PasswordSignInAsync(user, password, true, true);

        return result.Succeeded;
    }

    /// <summary>
    ///     Validates a user's password.
    /// </summary>
    public async Task<bool> ValidatePasswordAsync(Guid id, string password)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
            return false;

        return await userManager.CheckPasswordAsync(user, password);
    }

    /// <summary>
    ///     Validates an authorization code.
    /// </summary>
    public async Task<bool> ValidateAuthorizationCodeAsync(Guid userId, string code)
    {
        var user = await database.Users.FindAsync(userId);

        if (user?.AuthorizationCode is null)
            return false;

        var authorization = user.AuthorizationCode;

        if (DateTime.UtcNow - authorization.GeneratedAt >= TimeSpan.FromMinutes(15)) return false;

        var providedHash = SHA256.HashData(Encoding.UTF8.GetBytes(code));

        byte[] storedHash;

        try
        {
            storedHash = Convert.FromHexString(authorization.Code);
        }
        catch (FormatException)
        {
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(providedHash, storedHash);
    }

    /// <summary>
    ///     Generates a new authorization code.
    /// </summary>
    public async Task<string> RegenerateAuthorizationCodeAsync(Guid userId)
    {
        var user = await database.Users.FindAsync(userId) ?? throw new UserException(404, "User not found.");
        var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString("D6");
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(code));

        user.AuthorizationCode = new UserAuthorizationCode(Convert.ToHexString(hash), DateTime.UtcNow);

        await database.SaveChangesAsync();

        return code;
    }
}