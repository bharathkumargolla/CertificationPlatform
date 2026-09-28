using Certification.Application.Common.Models;
using Certification.Application.Common.Queries;
using Certification.Application.Questions.DTOs;
using Certification.Contracts.Common;

namespace Certification.Application.Questions.Queries.GetQuestions;

public sealed class GetQuestionsQuery : IQuery<ApiResponse<PagedResult<QuestionDto>>>
{
    public PagedRequest PagedRequest { get; init; } = new();

    public SearchRequest SearchRequest { get; init; } = new();

    public SortRequest SortRequest { get; init; } = new();
}
