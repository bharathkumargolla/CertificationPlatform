using Serilog;

namespace Certification.Api.Extensions;

public static class LoggingExtensions
{
    public static IHostBuilder AddApplicationLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        return hostBuilder.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithThreadId()
            .Enrich.WithProcessId()
            .WriteTo.Console()
            .WriteTo.File(
                path: "logs/log-.txt",
                rollingInterval: RollingInterval.Day));
    }
}
