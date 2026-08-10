using FluentValidation;
using FluentValidation.Results;

namespace Certification.Application.Common.Validation;

public abstract class BaseValidator<T> : AbstractValidator<T>
{
    protected static IReadOnlyList<ValidationError> ToValidationErrors(IEnumerable<ValidationFailure> failures) =>
        failures
            .Select(failure => new ValidationError(failure.PropertyName, failure.ErrorMessage))
            .ToList();
}
