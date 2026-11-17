using API.Contexts;
using API.DTO;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Validators;

/// <summary>
///     The validator for creating users.
/// </summary>
public sealed class UserCreateValidator : AbstractValidator<UserDto.Create>
{
    public UserCreateValidator(DatabaseContext database)
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3).MaximumLength(30).WithMessage("Invalid username");
        RuleFor(x => x.Username).MustAsync(async (username, cancellationToken) => !await database.Users.AnyAsync(user => user.UserName == username, cancellationToken))
            .WithMessage("The provided username is already taken.");

        RuleFor(x => x.Email).MustAsync(async (email, cancellationToken) => !await database.Users.AnyAsync(user => user.Email == email, cancellationToken))
            .WithMessage("The provided email address is already used for another account.");
    }
}