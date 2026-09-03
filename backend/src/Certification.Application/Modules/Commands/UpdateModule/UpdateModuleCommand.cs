using Certification.Application.Common.Commands;
using Certification.Application.Common.Results;

namespace Certification.Application.Modules.Commands.UpdateModule;

public sealed class UpdateModuleCommand : ICommand<Result>
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int DisplayOrder { get; init; }

    public bool IsActive { get; init; }
}
