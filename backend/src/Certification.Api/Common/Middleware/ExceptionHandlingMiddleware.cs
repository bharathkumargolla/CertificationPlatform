using Certification.Shared.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Certification.Api.Common.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var correlationId = context.Items[LoggingConstants.CorrelationId]?.ToString() ?? string.Empty;
        var statusCode = MapStatusCode(exception);

        _logger.LogError(
            exception,
            "Unhandled exception. CorrelationId: {CorrelationId}, Method: {RequestMethod}, Path: {RequestPath}, ExceptionType: {ExceptionType}, ExceptionMessage: {ExceptionMessage}",
            correlationId,
            context.Request.Method,
            context.Request.Path,
            exception.GetType().Name,
            exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = ReasonPhrases.GetReasonPhrase(statusCode),
            Detail = ResolveDetail(exception, statusCode),
            Instance = context.Request.Path,
        };

        problemDetails.Extensions[LoggingConstants.CorrelationId] = correlationId;

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static int MapStatusCode(Exception exception) => exception switch
    {
        ValidationException => StatusCodes.Status400BadRequest,
        UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
        KeyNotFoundException => StatusCodes.Status404NotFound,
        NotImplementedException => StatusCodes.Status501NotImplemented,
        _ => StatusCodes.Status500InternalServerError,
    };

    private static string ResolveDetail(Exception exception, int statusCode) =>
        statusCode == StatusCodes.Status500InternalServerError
            ? "An unexpected error occurred. Please contact support if the problem persists."
            : exception.Message;
}
