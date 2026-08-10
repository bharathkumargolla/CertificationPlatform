namespace Certification.Application.Common.Models;

public sealed class FilterRequest
{
    public Dictionary<string, string> Filters { get; set; } = [];
}
