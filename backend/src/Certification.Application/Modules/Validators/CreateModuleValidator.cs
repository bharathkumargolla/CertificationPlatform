using Certification.Application.Common.Validation;
using Certification.Application.Modules.Commands.CreateModule;
using FluentValidation;

namespace Certification.Application.Modules.Validators;

public sealed class CreateModuleValidator : BaseValidator<CreateModuleCommand>
{
    public CreateModuleValidator()
    {
        RuleFor(command => command.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.Description)
            .MaximumLength(1000);

        RuleFor(command => command.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }
}
