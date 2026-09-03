using System.Net;
using System.Net.Http.Json;
using Certification.Application.Common.Models;
using Certification.Application.Modules.DTOs;
using Certification.Contracts.Common;
using Certification.IntegrationTests.Common;
using FluentAssertions;

namespace Certification.IntegrationTests.Modules;

public sealed class ModulesEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ModulesEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetModules_ReturnsSuccessWithPagedResult()
    {
        var response = await _client.GetAsync("/api/v1/modules");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<ModuleDto>>>();

        payload.Should().NotBeNull();
        payload!.Success.Should().BeTrue();
        payload.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task PostModule_WithValidRequest_ReturnsCreated()
    {
        var request = new CreateModuleRequest
        {
            Code = $"MOD-{Guid.NewGuid():N}"[..12],
            Name = "Integration Test Module",
            Description = "Created by an integration test.",
            DisplayOrder = 1,
        };

        var response = await _client.PostAsJsonAsync("/api/v1/modules", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<Guid>>();

        payload.Should().NotBeNull();
        payload!.Success.Should().BeTrue();
        payload.Data.Should().NotBeEmpty();
    }
}
