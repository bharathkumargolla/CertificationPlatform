using Certification.Application.Common.Commands;
using Certification.Application.Common.Results;

namespace Certification.Application.Questions.Commands.DeleteQuestion;

public sealed class DeleteQuestionCommand : ICommand<Result>
{
    public Guid Id { get; init; }
}
