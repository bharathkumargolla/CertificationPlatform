using Certification.Application.Common.Queries;
using Certification.Application.Common.Results;
using Certification.Application.Questions.DTOs;

namespace Certification.Application.Questions.Queries.GetQuestion;

public sealed class GetQuestionQuery : IQuery<Result<QuestionDto>>
{
    public Guid Id { get; init; }
}
