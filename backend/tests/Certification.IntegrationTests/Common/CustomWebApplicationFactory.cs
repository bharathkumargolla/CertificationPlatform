using Certification.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Certification.IntegrationTests.Common;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    // AddDbContext registers DbContextOptions<TContext> as Scoped by default, so the
    // UseInMemoryDatabase name must be computed once per factory instance, not inside the
    // options-configuration lambda - otherwise every new DI scope (each HTTP request, and any
    // scope a test opens directly to seed data) would get its own randomly named, empty database.
    private readonly string _databaseName = $"IntegrationTests-{Guid.NewGuid()}";

    public CustomWebApplicationFactory()
    {
        // Program.cs reads ConnectionStrings:DefaultConnection and Jwt:SecretKey directly from
        // builder.Configuration before builder.Build() is reached, so WebApplicationFactory's
        // ConfigureAppConfiguration hook (which only applies at Build()) runs too late for them.
        // Environment variables are read synchronously by WebApplicationBuilder.CreateBuilder,
        // so they are the only override visible in time.
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", "Host=localhost;Database=certification-tests;Username=test;Password=test");
        Environment.SetEnvironmentVariable("Jwt__SecretKey", "integration-test-signing-key-not-for-production-use");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // AddDbContext<ApplicationDbContext> was already called (with the Npgsql provider) by
            // AddInfrastructureServices. EF Core composes every registered options-configuration
            // for a given context type rather than replacing it, so removing only
            // DbContextOptions<ApplicationDbContext> leaves the Npgsql provider services behind
            // and registering UseInMemoryDatabase on top produces a "two providers registered"
            // error. Strip every service descriptor tied to ApplicationDbContext first.
            var dbContextDescriptors = services
                .Where(descriptor => descriptor.ServiceType.IsGenericType
                    && descriptor.ServiceType.GetGenericArguments().Contains(typeof(ApplicationDbContext)))
                .ToList();

            foreach (var descriptor in dbContextDescriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));

            services.AddAuthentication(TestAuthenticationHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                    TestAuthenticationHandler.SchemeName,
                    _ => { });

            services.PostConfigure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthenticationHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthenticationHandler.SchemeName;
                options.DefaultScheme = TestAuthenticationHandler.SchemeName;
            });
        });
    }
}
