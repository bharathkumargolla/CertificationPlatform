using Certification.Application.Common.Commands;
using Certification.Application.Common.Results;

namespace Certification.Application.Modules.Commands.DeleteModule;

public sealed class DeleteModuleCommand : ICommand<Result>
{
    public Guid Id { get; init; }
}
