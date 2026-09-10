using Certification.Application.Common.Commands;
using Certification.Application.Common.Results;

namespace Certification.Application.Certifications.Commands.DeleteCertification;

public sealed class DeleteCertificationCommand : ICommand<Result>
{
    public Guid Id { get; init; }
}
