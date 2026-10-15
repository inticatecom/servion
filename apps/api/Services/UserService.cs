using API.Contexts;
using API.Exceptions;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

/// <summary>
/// A service for managing users in the database.
/// </summary>
/// <param name="database">The database SDK.</param>
public sealed class UserService(DatabaseContext database)
{
    /// <summary>
    /// The hasher for hashing user's passwords to be stored in the database.
    /// </summary>
    private readonly PasswordHasher<UserModel> _hasher = new();

    /// <summary>
    /// Creates a new user with the provided information.
    /// </summary>
    /// <param name="username">The name of the account.</param>
    /// <param name="email">The email of the account.</param>
    /// <param name="password">The raw password to be hashed once the account is created.</param>
    public async Task CreateUser(string username, string email, string password)
    {
        var user = new UserModel
        {
            Username = username,
            Email = email,
            PasswordHash = ""
        };

        user.PasswordHash = _hasher.HashPassword(user, password);

        database.Users.Add(user);
        await database.SaveChangesAsync();
    }

    /// <summary>
    /// Compares a raw password by decrypting it securely.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="password">The raw password to compare.</param>
    /// <returns>Whether the password is accepted for the account.</returns>
    public async Task<bool> ValidatePassword(Guid id, string password)
    {
        var user = await database.Users.FindAsync(id);
        if (user is null)
        {
            return false;
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);

        return result != PasswordVerificationResult.Failed;
    }

    public async Task<bool> ValidateAuthorizationCode(Guid userId, string code)
    {
        var user = await database.Users.Where(user => user.Id == userId).Include(user => user.AuthorizationCode)
            .FirstAsync() ?? throw new UserException(404, "User not found.");

        if (user.AuthorizationCode is null)
        {
            return false;
        }

        return DateTime.UtcNow - user.AuthorizationCode.GeneratedAt >= TimeSpan.FromMinutes(15);
    }

    public async Task<string> RegenerateAuthorizationCode(Guid userId)
    {
        var user = await database.Users.Where(user => user.Id == userId).Include(user => user.AuthorizationCode)
            .FirstAsync() ?? throw new UserException(404, "User not found.");
        var code = Random.Shared.Next(100000, 999999).ToString();

        user.AuthorizationCode = new UserAuthorizationCode(code, DateTime.UtcNow);
        await database.SaveChangesAsync();

        return code;
    }
}