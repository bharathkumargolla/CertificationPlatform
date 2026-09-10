using System.Net;
using System.Net.Http.Json;
using Certification.Application.Certifications.DTOs;
using Certification.Application.Common.Models;
using Certification.Contracts.Common;
using Certification.Domain.Entities;
using Certification.Infrastructure.Persistence.Context;
using Certification.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Certification.IntegrationTests.Certifications;

public sealed class CertificationsEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CertificationsEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task<Guid> SeedActiveModuleAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var module = new Module
        {
            Code = $"MOD-{Guid.NewGuid():N}"[..12],
            Name = "Integration Test Module",
            IsActive = true,
        };
        dbContext.Modules.Add(module);
        await dbContext.SaveChangesAsync();

        return module.Id;
    }

    [Fact]
    public async Task GetCertifications_ReturnsSuccessWithPagedResult()
    {
        var response = await _client.GetAsync("/api/v1/certifications");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<CertificationDto>>>();

        payload.Should().NotBeNull();
        payload!.Success.Should().BeTrue();
        payload.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task PostCertification_WithValidRequest_ReturnsCreated()
    {
        var moduleId = await SeedActiveModuleAsync();

        var request = new CreateCertificationRequest
        {
            ModuleId = moduleId,
            Code = $"CERT-{Guid.NewGuid():N}"[..13],
            Name = "Integration Test Certification",
            DurationInMinutes = 90,
            PassingScore = 70,
            CertificateValidityMonths = 12,
            DisplayOrder = 1,
        };

        var response = await _client.PostAsJsonAsync("/api/v1/certifications", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<Guid>>();

        payload.Should().NotBeNull();
        payload!.Success.Should().BeTrue();
        payload.Data.Should().NotBeEmpty();
    }

    [Fact]
    public async Task PostCertification_WithInvalidRequest_ReturnsBadRequest()
    {
        var request = new CreateCertificationRequest
        {
            ModuleId = Guid.Empty,
            Code = string.Empty,
            Name = string.Empty,
            DurationInMinutes = 0,
            PassingScore = 150,
            CertificateValidityMonths = -1,
        };

        var response = await _client.PostAsJsonAsync("/api/v1/certifications", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteCertification_AsAdminWithoutSuperAdminRole_ReturnsForbidden()
    {
        // TestAuthenticationHandler authenticates every request as an Admin-role user;
        // DELETE requires SuperAdminOnly, so this must fail authorization, not authentication.
        var response = await _client.DeleteAsync($"/api/v1/certifications/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
