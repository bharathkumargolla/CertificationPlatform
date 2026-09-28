using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Results;
using Certification.Application.Questions.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Questions.Queries.GetQuestion;

public sealed class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, Result<QuestionDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapperService _mapperService;

    public GetQuestionQueryHandler(IApplicationDbContext dbContext, IMapperService mapperService)
    {
        _dbContext = dbContext;
        _mapperService = mapperService;
    }

    public async Task<Result<QuestionDto>> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
    {
        var question = await _dbContext.Questions
            .Include(existing => existing.QuestionOptions)
            .FirstOrDefaultAsync(existing => existing.Id == request.Id && !existing.IsDeleted, cancellationToken);

        if (question is null)
        {
            return Result.Failure<QuestionDto>($"Question with id '{request.Id}' was not found.");
        }

        return Result.Success(_mapperService.Map<QuestionDto>(question));
    }
}
