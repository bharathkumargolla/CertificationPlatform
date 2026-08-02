using Certification.Api.Common.Extensions;
using Certification.Api.Extensions;
using Certification.Application;
using Certification.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddApplicationLogging(builder.Configuration);

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseCorrelationId();
app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseCors(ServiceCollectionExtensions.CorsPolicyName);

app.MapHealthChecks("/health");

app.Run();
