using API.DTO;
using FluentValidation;
using JetBrains.Annotations;

namespace API.Validators;

[UsedImplicitly]
public sealed class ServerCreateValidator : AbstractValidator<ServerDto.Create>
{
    public ServerCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(30);
    }
}

[UsedImplicitly]
public sealed class ServerSetStateValidator : AbstractValidator<ServerDto.SetState>
{
    public ServerSetStateValidator()
    {
        // TODO: Update request to take string for enum (eg. 'Online' -> ServerState.Online).
        RuleFor(x => x.State);
    }
}