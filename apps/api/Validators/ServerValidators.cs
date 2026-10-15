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