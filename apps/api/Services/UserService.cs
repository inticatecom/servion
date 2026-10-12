using API.Contexts;
using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Services;

public sealed class UserService(DatabaseContext database)
{
    private readonly PasswordHasher<UserModel> _hasher = new();

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
}