using Certification.Application.Common.Extensions;
using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Models;
using Certification.Application.Questions.DTOs;
using Certification.Contracts.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Questions.Queries.GetQuestions;

public sealed class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, ApiResponse<PagedResult<QuestionDto>>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapperService _mapperService;

    public GetQuestionsQueryHandler(IApplicationDbContext dbContext, IMapperService mapperService)
    {
        _dbContext = dbContext;
        _mapperService = mapperService;
    }

    public async Task<ApiResponse<PagedResult<QuestionDto>>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Questions.Where(question => !question.IsDeleted);

        var search = request.SearchRequest.Search;
        if (search is not null)
        {
            query = query.Where(question => question.QuestionText.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var questions = await query
            .Include(question => question.QuestionOptions)
            .OrderBy(question => question.DisplayOrder)
            .ThenBy(question => question.QuestionText)
            .ApplyPaging(request.PagedRequest)
            .ToListAsync(cancellationToken);

        var pagedResult = new PagedResult<QuestionDto>
        {
            Items = questions.Select(question => _mapperService.Map<QuestionDto>(question)).ToList(),
            TotalCount = totalCount,
            PageNumber = request.PagedRequest.PageNumber,
            PageSize = request.PagedRequest.PageSize,
        };

        return PagedResponse.Create(pagedResult);
    }
}
