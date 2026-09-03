namespace Certification.Application.Modules.DTOs;

public sealed class UpdateModuleRequest
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int DisplayOrder { get; init; }

    public bool IsActive { get; init; }
}
