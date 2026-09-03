using Asp.Versioning;
using Certification.Application.Common.Models;
using Certification.Application.Modules.Commands.CreateModule;
using Certification.Application.Modules.Commands.DeleteModule;
using Certification.Application.Modules.Commands.UpdateModule;
using Certification.Application.Modules.DTOs;
using Certification.Application.Modules.Queries.GetModule;
using Certification.Application.Modules.Queries.GetModules;
using Certification.Contracts.Common;
using Certification.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Certification.Api.Controllers;

/// <summary>Manages certification modules.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/modules")]
public sealed class ModulesController : ControllerBase
{
    private readonly ISender _sender;

    public ModulesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>Gets a paged list of modules.</summary>
    [HttpGet]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Trainer}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ModuleDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetModules(
        [FromQuery] PagedRequest pagedRequest,
        [FromQuery] SearchRequest searchRequest,
        [FromQuery] SortRequest sortRequest,
        CancellationToken cancellationToken)
    {
        var query = new GetModulesQuery
        {
            PagedRequest = pagedRequest,
            SearchRequest = searchRequest,
            SortRequest = sortRequest,
        };

        var response = await _sender.Send(query, cancellationToken);
        return Ok(response);
    }

    /// <summary>Gets a single module by id.</summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Trainer}")]
    [ProducesResponseType(typeof(ApiResponse<ModuleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetModule(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetModuleQuery { Id = id }, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error, statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(ApiResponse.Success(result.Value));
    }

    /// <summary>Creates a new module.</summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateModule([FromBody] CreateModuleRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateModuleCommand
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            DisplayOrder = request.DisplayOrder,
        };

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error, statusCode: StatusCodes.Status409Conflict);
        }

        return CreatedAtAction(nameof(GetModule), new { id = result.Value }, ApiResponse.Success(result.Value));
    }

    /// <summary>Updates an existing module.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateModule(Guid id, [FromBody] UpdateModuleRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateModuleCommand
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            DisplayOrder = request.DisplayOrder,
            IsActive = request.IsActive,
        };

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error, statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(ApiResponse.Success<object?>(null));
    }

    /// <summary>Soft-deletes a module.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.SuperAdminOnly)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteModule(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteModuleCommand { Id = id }, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error, statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(ApiResponse.Success<object?>(null));
    }
}
