using Certification.Application.Common.Validation;
using Certification.Application.Questions.Commands.UpdateQuestion;
using Certification.Domain.Enums;
using FluentValidation;

namespace Certification.Application.Questions.Validators;

public sealed class UpdateQuestionValidator : BaseValidator<UpdateQuestionCommand>
{
    public UpdateQuestionValidator()
    {
        RuleFor(command => command.QuestionText)
            .NotEmpty()
            .MaximumLength(4000);

        RuleFor(command => command.Explanation)
            .MaximumLength(4000);

        RuleFor(command => command.Points)
            .GreaterThan(0);

        RuleFor(command => command.Options)
            .Must(QuestionOptionRules.HasValidOptionCount)
            .WithMessage($"A question must have between {QuestionOptionRules.MinimumOptionCount} and {QuestionOptionRules.MaximumOptionCount} options.");

        RuleFor(command => command.Options)
            .Must(QuestionOptionRules.HasAtLeastOneCorrectOption)
            .WithMessage("A question must have at least one correct option.");

        RuleFor(command => command)
            .Must(command => QuestionOptionRules.SatisfiesQuestionTypeRules(command.QuestionType, command.Options))
            .WithMessage(command => command.QuestionType switch
            {
                QuestionType.SingleChoice => "Single choice questions must have exactly one correct option.",
                QuestionType.MultipleChoice => "Multiple choice questions must have at least one correct option.",
                QuestionType.TrueFalse => "True/false questions must have exactly two options, 'True' and 'False'.",
                _ => "Question options are invalid for the selected question type.",
            });
    }
}
