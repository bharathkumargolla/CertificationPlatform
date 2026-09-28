using Certification.Application.Questions.DTOs;
using Certification.Domain.Enums;

namespace Certification.Application.Questions.Validators;

internal static class QuestionOptionRules
{
    internal const int MinimumOptionCount = 2;
    internal const int MaximumOptionCount = 6;

    internal static bool HasValidOptionCount(IReadOnlyList<QuestionOptionDto> options) =>
        options.Count is >= MinimumOptionCount and <= MaximumOptionCount;

    internal static bool HasAtLeastOneCorrectOption(IReadOnlyList<QuestionOptionDto> options) =>
        options.Any(option => option.IsCorrect);

    internal static bool SatisfiesQuestionTypeRules(QuestionType questionType, IReadOnlyList<QuestionOptionDto> options)
    {
        var correctCount = options.Count(option => option.IsCorrect);

        return questionType switch
        {
            QuestionType.SingleChoice => correctCount == 1,
            QuestionType.MultipleChoice => correctCount >= 1,
            QuestionType.TrueFalse => options.Count == 2 && HasTrueAndFalseOptionText(options),
            _ => true,
        };
    }

    private static bool HasTrueAndFalseOptionText(IReadOnlyList<QuestionOptionDto> options) =>
        options.Any(option => string.Equals(option.OptionText, "True", StringComparison.OrdinalIgnoreCase))
        && options.Any(option => string.Equals(option.OptionText, "False", StringComparison.OrdinalIgnoreCase));
}
