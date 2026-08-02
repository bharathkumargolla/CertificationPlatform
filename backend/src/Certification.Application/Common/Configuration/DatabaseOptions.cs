using System.ComponentModel.DataAnnotations;

namespace Certification.Application.Common.Configuration;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    [Range(1, 300)]
    public int CommandTimeoutSeconds { get; init; } = 30;

    public bool EnableSensitiveDataLogging { get; init; }

    [Range(0, 10)]
    public int MaxRetryCount { get; init; } = 3;
}
