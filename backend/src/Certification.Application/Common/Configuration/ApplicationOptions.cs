using System.ComponentModel.DataAnnotations;

namespace Certification.Application.Common.Configuration;

public sealed class ApplicationOptions
{
    public const string SectionName = "Application";

    [Required]
    public string Name { get; init; } = string.Empty;

    [Required]
    [Url]
    public string Url { get; init; } = string.Empty;

    [EmailAddress]
    public string SupportEmail { get; init; } = string.Empty;
}
