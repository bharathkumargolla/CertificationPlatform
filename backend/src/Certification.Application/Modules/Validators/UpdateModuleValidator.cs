using Certification.Application.Common.Validation;
using Certification.Application.Modules.Commands.UpdateModule;
using FluentValidation;

namespace Certification.Application.Modules.Validators;

public sealed class UpdateModuleValidator : BaseValidator<UpdateModuleCommand>
{
    public UpdateModuleValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.Description)
            .MaximumLength(1000);

        RuleFor(command => command.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }
}
