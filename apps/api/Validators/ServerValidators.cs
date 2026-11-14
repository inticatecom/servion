using API.DTO;
using FluentValidation;
using JetBrains.Annotations;

namespace API.Validators;

/// <summary>
///     The validator for creating a server.
/// </summary>
[UsedImplicitly]
public sealed class ServerCreateValidator : AbstractValidator<ServerDto.Create>
{
    public ServerCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(30).WithMessage("Invalid server name.");
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