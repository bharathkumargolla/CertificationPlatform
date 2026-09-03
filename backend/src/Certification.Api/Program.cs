using Certification.Api.Common.Extensions;
using Certification.Api.Common.HealthChecks;
using Certification.Api.Extensions;
using Certification.Application;
using Certification.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddApplicationLogging(builder.Configuration);

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration)
    .AddSwaggerDocumentation();

var app = builder.Build();

app.UseCorrelationId();
app.UseSecurityHeaders();
app.UseGlobalExceptionHandler();

app.UseSwaggerDocumentation();

app.UseSerilogRequestLogging();

app.UseCors(ServiceCollectionExtensions.CorsPolicyName);

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = app.Environment.IsDevelopment()
        ? HealthCheckResponseWriter.WriteDetailedAsync
        : HealthCheckResponseWriter.WriteMinimalAsync,
});
app.MapControllers();

app.Run();

public partial class Program;
