namespace Certification.Application.Common.Models;

public sealed class PagedRequest
{
    public const int MinimumPageNumber = 1;

    public const int DefaultPageSize = 20;

    public const int MaximumPageSize = 100;

    private int _pageNumber = MinimumPageNumber;
    private int _pageSize = DefaultPageSize;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = Math.Max(value, MinimumPageNumber);
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? DefaultPageSize : Math.Min(value, MaximumPageSize);
    }
}
