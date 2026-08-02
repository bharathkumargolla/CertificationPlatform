using Certification.Shared.Constants;

namespace Certification.Api.Common.Extensions;

public static class HttpContextExtensions
{
    public static string GetCorrelationId(this HttpContext context) =>
        context.Items[LoggingConstants.CorrelationId]?.ToString() ?? string.Empty;
}
