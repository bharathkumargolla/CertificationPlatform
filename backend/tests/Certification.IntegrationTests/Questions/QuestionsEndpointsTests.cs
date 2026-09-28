using System.Net;
using System.Net.Http.Json;
using Certification.Application.Common.Models;
using Certification.Application.Questions.DTOs;
using Certification.Contracts.Common;
using Certification.Domain.Entities;
using Certification.Domain.Enums;
using Certification.Infrastructure.Persistence.Context;
using Certification.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Certification.IntegrationTests.Questions;

public sealed class QuestionsEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public QuestionsEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task<Guid> SeedActiveCertificationAsync()
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

        var certification = new CertificationDefinition
        {
            ModuleId = module.Id,
            Code = $"CERT-{Guid.NewGuid():N}"[..13],
            Name = "Integration Test Certification",
            DurationInMinutes = 90,
            PassingScore = 70,
            IsActive = true,
        };
        dbContext.Certifications.Add(certification);
        await dbContext.SaveChangesAsync();

        return certification.Id;
    }

    private async Task<Guid> SeedQuestionAsync(Guid certificationDefinitionId)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var question = new Question
        {
            CertificationDefinitionId = certificationDefinitionId,
            QuestionText = "Seeded question",
            QuestionType = QuestionType.SingleChoice,
            DifficultyLevel = DifficultyLevel.Easy,
            Points = 10,
        };
        question.QuestionOptions.Add(new QuestionOption { OptionText = "A", IsCorrect = true });
        question.QuestionOptions.Add(new QuestionOption { OptionText = "B", IsCorrect = false });

        dbContext.Questions.Add(question);
        await dbContext.SaveChangesAsync();

        return question.Id;
    }

    private static List<QuestionOptionDto> ValidOptions() =>
    [
        new QuestionOptionDto { OptionText = "Option A", IsCorrect = true },
        new QuestionOptionDto { OptionText = "Option B", IsCorrect = false },
    ];

    [Fact]
    public async Task GetQuestions_ReturnsSuccessWithPagedResult()
    {
        var response = await _client.GetAsync("/api/v1/questions");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<QuestionDto>>>();

        payload.Should().NotBeNull();
        payload!.Success.Should().BeTrue();
        payload.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetQuestion_ForExistingQuestion_ReturnsQuestionWithOptions()
    {
        var certificationId = await SeedActiveCertificationAsync();
        var questionId = await SeedQuestionAsync(certificationId);

        var response = await _client.GetAsync($"/api/v1/questions/{questionId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<QuestionDto>>();

        payload.Should().NotBeNull();
        payload!.Success.Should().BeTrue();
        payload.Data.Should().NotBeNull();
        payload.Data!.Options.Should().HaveCount(2);
    }

    [Fact]
    public async Task PostQuestion_WithValidRequest_ReturnsCreated()
    {
        var certificationId = await SeedActiveCertificationAsync();

        var request = new CreateQuestionRequest
        {
            CertificationDefinitionId = certificationId,
            QuestionText = "What is the capital of France?",
            QuestionType = QuestionType.SingleChoice,
            DifficultyLevel = DifficultyLevel.Easy,
            Points = 10,
            Options = ValidOptions(),
        };

        var response = await _client.PostAsJsonAsync("/api/v1/questions", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<Guid>>();

        payload.Should().NotBeNull();
        payload!.Success.Should().BeTrue();
        payload.Data.Should().NotBeEmpty();
    }

    [Fact]
    public async Task PostQuestion_WithInvalidOptionCount_ReturnsBadRequest()
    {
        var certificationId = await SeedActiveCertificationAsync();

        var request = new CreateQuestionRequest
        {
            CertificationDefinitionId = certificationId,
            QuestionText = "Question with only one option",
            QuestionType = QuestionType.SingleChoice,
            Points = 10,
            Options = [new QuestionOptionDto { OptionText = "Only option", IsCorrect = true }],
        };

        var response = await _client.PostAsJsonAsync("/api/v1/questions", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutQuestion_WithValidRequest_ReturnsSuccess()
    {
        var certificationId = await SeedActiveCertificationAsync();
        var questionId = await SeedQuestionAsync(certificationId);

        var request = new UpdateQuestionRequest
        {
            QuestionText = "Updated question text",
            QuestionType = QuestionType.SingleChoice,
            DifficultyLevel = DifficultyLevel.Medium,
            Points = 15,
            IsActive = true,
            Options = ValidOptions(),
        };

        var response = await _client.PutAsJsonAsync($"/api/v1/questions/{questionId}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await _client.GetAsync($"/api/v1/questions/{questionId}");
        var payload = await getResponse.Content.ReadFromJsonAsync<ApiResponse<QuestionDto>>();

        payload!.Data!.QuestionText.Should().Be("Updated question text");
        payload.Data.Points.Should().Be(15);
    }

    [Fact]
    public async Task DeleteQuestion_AsAdminWithoutSuperAdminRole_ReturnsForbidden()
    {
        var response = await _client.DeleteAsync($"/api/v1/questions/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteQuestion_AsSuperAdmin_ReturnsSuccess()
    {
        var certificationId = await SeedActiveCertificationAsync();
        var questionId = await SeedQuestionAsync(certificationId);

        using var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/questions/{questionId}");
        request.Headers.Add(TestAuthenticationHandler.RoleHeaderName, "SuperAdmin");

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
