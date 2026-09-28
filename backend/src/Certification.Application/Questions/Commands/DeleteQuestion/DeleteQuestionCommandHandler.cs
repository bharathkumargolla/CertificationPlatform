using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Questions.Commands.DeleteQuestion;

public sealed class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteQuestionCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _dbContext.Questions
            .FirstOrDefaultAsync(existing => existing.Id == request.Id && !existing.IsDeleted, cancellationToken);

        if (question is null)
        {
            return Result.Failure($"Question with id '{request.Id}' was not found.");
        }

        question.IsDeleted = true;
        question.DeletedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
