using Certification.Contracts.Common;

namespace Certification.Application.Common.Models;

public static class PagedResponse
{
    public static ApiResponse<PagedResult<T>> Create<T>(PagedResult<T> pagedResult) =>
        ApiResponse.Success(pagedResult);
}
