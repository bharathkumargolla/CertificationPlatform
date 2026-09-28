using Certification.Application.Questions.Commands.CreateQuestion;
using Certification.Application.Questions.DTOs;
using Certification.Application.Questions.Validators;
using Certification.Domain.Enums;
using FluentValidation.TestHelper;

namespace Certification.UnitTests.Questions;

public sealed class CreateQuestionValidatorTests
{
    private readonly CreateQuestionValidator _validator = new();

    private static CreateQuestionCommand ValidCommand(QuestionType questionType, List<QuestionOptionDto> options) => new()
    {
        CertificationDefinitionId = Guid.NewGuid(),
        QuestionText = "Sample question",
        QuestionType = questionType,
        Points = 10,
        Options = options,
    };

    [Fact]
    public void Validate_WithValidSingleChoiceQuestion_HasNoValidationErrors()
    {
        var command = ValidCommand(
            QuestionType.SingleChoice,
            [
                new QuestionOptionDto { OptionText = "A", IsCorrect = true },
                new QuestionOptionDto { OptionText = "B", IsCorrect = false },
            ]);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithTooFewOptions_HasValidationError()
    {
        var command = ValidCommand(
            QuestionType.SingleChoice,
            [
                new QuestionOptionDto { OptionText = "A", IsCorrect = true },
            ]);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Options);
    }

    [Fact]
    public void Validate_WithTooManyOptions_HasValidationError()
    {
        var options = Enumerable.Range(1, 7)
            .Select(i => new QuestionOptionDto { OptionText = $"Option {i}", IsCorrect = i == 1 })
            .ToList();

        var command = ValidCommand(QuestionType.MultipleChoice, options);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Options);
    }

    [Fact]
    public void Validate_WithNoCorrectOption_HasValidationError()
    {
        var command = ValidCommand(
            QuestionType.SingleChoice,
            [
                new QuestionOptionDto { OptionText = "A", IsCorrect = false },
                new QuestionOptionDto { OptionText = "B", IsCorrect = false },
            ]);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Options);
    }

    [Fact]
    public void Validate_SingleChoiceWithMultipleCorrectOptions_HasValidationError()
    {
        var command = ValidCommand(
            QuestionType.SingleChoice,
            [
                new QuestionOptionDto { OptionText = "A", IsCorrect = true },
                new QuestionOptionDto { OptionText = "B", IsCorrect = true },
            ]);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c);
    }

    [Fact]
    public void Validate_TrueFalseWithWrongOptionText_HasValidationError()
    {
        var command = ValidCommand(
            QuestionType.TrueFalse,
            [
                new QuestionOptionDto { OptionText = "Yes", IsCorrect = true },
                new QuestionOptionDto { OptionText = "No", IsCorrect = false },
            ]);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c);
    }

    [Fact]
    public void Validate_ValidTrueFalseQuestion_HasNoValidationErrors()
    {
        var command = ValidCommand(
            QuestionType.TrueFalse,
            [
                new QuestionOptionDto { OptionText = "True", IsCorrect = true },
                new QuestionOptionDto { OptionText = "False", IsCorrect = false },
            ]);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
