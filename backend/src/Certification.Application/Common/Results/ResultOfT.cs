namespace Certification.Application.Common.Results;

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    private Result(bool isSuccess, TValue? value, string? error, IReadOnlyList<string> validationErrors)
        : base(isSuccess, error, validationErrors)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access the value of a failed result.");

    public static Result<TValue> Success(TValue value) => new(true, value, error: null, validationErrors: []);

    public static new Result<TValue> Failure(string error) => new(false, default, error, validationErrors: []);

    public static new Result<TValue> Failure(IReadOnlyList<string> validationErrors) =>
        new(false, default, "One or more validation errors occurred.", validationErrors);
}
