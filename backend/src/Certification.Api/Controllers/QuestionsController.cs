using Asp.Versioning;
using Certification.Application.Common.Models;
using Certification.Application.Questions.Commands.CreateQuestion;
using Certification.Application.Questions.Commands.DeleteQuestion;
using Certification.Application.Questions.Commands.UpdateQuestion;
using Certification.Application.Questions.DTOs;
using Certification.Application.Questions.Queries.GetQuestion;
using Certification.Application.Questions.Queries.GetQuestions;
using Certification.Contracts.Common;
using Certification.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Certification.Api.Controllers;

/// <summary>Manages the question bank for certifications.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/questions")]
public sealed class QuestionsController : ControllerBase
{
    private readonly ISender _sender;

    public QuestionsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>Gets a paged list of questions.</summary>
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.AdminOrTrainer)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<QuestionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuestions(
        [FromQuery] PagedRequest pagedRequest,
        [FromQuery] SearchRequest searchRequest,
        [FromQuery] SortRequest sortRequest,
        CancellationToken cancellationToken)
    {
        var query = new GetQuestionsQuery
        {
            PagedRequest = pagedRequest,
            SearchRequest = searchRequest,
            SortRequest = sortRequest,
        };

        var response = await _sender.Send(query, cancellationToken);
        return Ok(response);
    }

    /// <summary>Gets a single question, including its options, by id.</summary>
    [HttpGet("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AdminOrTrainer)]
    [ProducesResponseType(typeof(ApiResponse<QuestionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestion(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetQuestionQuery { Id = id }, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error, statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(ApiResponse.Success(result.Value));
    }

    /// <summary>Creates a new question with its options.</summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateQuestionCommand
        {
            CertificationDefinitionId = request.CertificationDefinitionId,
            QuestionText = request.QuestionText,
            Explanation = request.Explanation,
            QuestionType = request.QuestionType,
            DifficultyLevel = request.DifficultyLevel,
            Points = request.Points,
            DisplayOrder = request.DisplayOrder,
            Options = request.Options,
        };

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error, statusCode: StatusCodes.Status400BadRequest);
        }

        return CreatedAtAction(nameof(GetQuestion), new { id = result.Value }, ApiResponse.Success(result.Value));
    }

    /// <summary>Updates an existing question, replacing its options.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateQuestion(Guid id, [FromBody] UpdateQuestionRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateQuestionCommand
        {
            Id = id,
            QuestionText = request.QuestionText,
            Explanation = request.Explanation,
            QuestionType = request.QuestionType,
            DifficultyLevel = request.DifficultyLevel,
            Points = request.Points,
            DisplayOrder = request.DisplayOrder,
            IsActive = request.IsActive,
            Options = request.Options,
        };

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error, statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(ApiResponse.Success<object?>(null));
    }

    /// <summary>Soft-deletes a question.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.SuperAdminOnly)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuestion(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteQuestionCommand { Id = id }, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error, statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(ApiResponse.Success<object?>(null));
    }
}
