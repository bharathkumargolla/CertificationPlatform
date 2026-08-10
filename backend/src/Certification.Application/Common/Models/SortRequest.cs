namespace Certification.Application.Common.Models;

public sealed class SortRequest
{
    public string? SortBy { get; set; }

    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
}

public enum SortDirection
{
    Ascending,
    Descending,
}
