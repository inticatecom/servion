using API.Contexts;
using API.DTO;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace API.Validators;

/// <summary>
///     The validator for creating a server.
/// </summary>
[UsedImplicitly]
public sealed class ServerCreateValidator : AbstractValidator<ServerDto.Create>
{
    public ServerCreateValidator(DatabaseContext database)
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(30).WithMessage("Invalid server name.");

        RuleFor(x => x.BackendId).MustAsync(async (id, cancellationToken) => await database.Backends.AnyAsync(backend => backend.Id == id, cancellationToken))
            .WithMessage("Invalid backend ID.");
    }
}

/// <summary>
///     The validator for setting the state of a server.
/// </summary>
[UsedImplicitly]
public sealed class ServerSetStateValidator : AbstractValidator<ServerDto.SetState>
{
    public ServerSetStateValidator()
    {
        RuleFor(x => x.State).IsInEnum().WithMessage("Invalid server state.");
    }
}