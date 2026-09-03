using Certification.Application.Common.Commands;
using Certification.Application.Common.Results;

namespace Certification.Application.Modules.Commands.CreateModule;

public sealed class CreateModuleCommand : ICommand<Result<Guid>>
{
    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int DisplayOrder { get; init; }
}
