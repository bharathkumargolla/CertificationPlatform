namespace Certification.Application.Common.Results;

public class Result
{
    protected Result(bool isSuccess, string? error, IReadOnlyList<string> validationErrors)
    {
        IsSuccess = isSuccess;
        Error = error;
        ValidationErrors = validationErrors;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public string? Error { get; }

    public IReadOnlyList<string> ValidationErrors { get; }

    public static Result Success() => new(true, error: null, validationErrors: []);

    public static Result Failure(string error) => new(false, error, validationErrors: []);

    public static Result Failure(IReadOnlyList<string> validationErrors) =>
        new(false, "One or more validation errors occurred.", validationErrors);

    public static Result<TValue> Success<TValue>(TValue value) => Result<TValue>.Success(value);

    public static Result<TValue> Failure<TValue>(string error) => Result<TValue>.Failure(error);

    public static Result<TValue> Failure<TValue>(IReadOnlyList<string> validationErrors) =>
        Result<TValue>.Failure(validationErrors);
}
