namespace Certification.Application.Modules.DTOs;

public sealed class CreateModuleRequest
{
    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int DisplayOrder { get; init; }
}
