namespace Certification.Application.Common.Models;

public sealed class SearchRequest
{
    private string? _search;

    public string? Search
    {
        get => _search;
        set => _search = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
