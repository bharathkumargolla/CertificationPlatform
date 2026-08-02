namespace Certification.Contracts.Common;

public sealed class ApiResponse<T>
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;

    public T? Data { get; init; }

    public DateTime TimestampUtc { get; init; } = DateTime.UtcNow;

    public string CorrelationId { get; init; } = string.Empty;
}

public static class ApiResponse
{
    public static ApiResponse<T> Success<T>(T data) => new()
    {
        Success = true,
        Data = data,
    };

    public static ApiResponse<T> Success<T>(string message, T data) => new()
    {
        Success = true,
        Message = message,
        Data = data,
    };

    public static ApiResponse<T> Failure<T>(string message) => new()
    {
        Success = false,
        Message = message,
    };

    public static ApiResponse<T> Failure<T>(string message, string correlationId) => new()
    {
        Success = false,
        Message = message,
        CorrelationId = correlationId,
    };
}
