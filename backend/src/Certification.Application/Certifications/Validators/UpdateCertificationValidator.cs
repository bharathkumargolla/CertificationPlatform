using Certification.Application.Certifications.Commands.UpdateCertification;
using Certification.Application.Common.Validation;
using FluentValidation;

namespace Certification.Application.Certifications.Validators;

public sealed class UpdateCertificationValidator : BaseValidator<UpdateCertificationCommand>
{
    public UpdateCertificationValidator()
    {
        RuleFor(command => command.ModuleId)
            .NotEmpty();

        RuleFor(command => command.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.Vendor)
            .MaximumLength(100);

        RuleFor(command => command.Version)
            .MaximumLength(50);

        RuleFor(command => command.Description)
            .MaximumLength(1000);

        RuleFor(command => command.DurationInMinutes)
            .GreaterThan(0);

        RuleFor(command => command.PassingScore)
            .InclusiveBetween(0, 100);

        RuleFor(command => command.CertificateValidityMonths)
            .GreaterThanOrEqualTo(0);
    }
}
