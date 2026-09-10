using Asp.Versioning;
using Certification.Application.Certifications.Commands.CreateCertification;
using Certification.Application.Certifications.Commands.DeleteCertification;
using Certification.Application.Certifications.Commands.UpdateCertification;
using Certification.Application.Certifications.DTOs;
using Certification.Application.Certifications.Queries.GetCertification;
using Certification.Application.Certifications.Queries.GetCertifications;
using Certification.Application.Common.Models;
using Certification.Contracts.Common;
using Certification.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Certification.Api.Controllers;

/// <summary>Manages certifications offered under a module.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/certifications")]
public sealed class CertificationsController : ControllerBase
{
    private readonly ISender _sender;

    public CertificationsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>Gets a paged list of certifications.</summary>
    [HttpGet]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Trainer}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<CertificationDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCertifications(
        [FromQuery] PagedRequest pagedRequest,
        [FromQuery] SearchRequest searchRequest,
        [FromQuery] SortRequest sortRequest,
        CancellationToken cancellationToken)
    {
        var query = new GetCertificationsQuery
        {
            PagedRequest = pagedRequest,
            SearchRequest = searchRequest,
            SortRequest = sortRequest,
        };

        var response = await _sender.Send(query, cancellationToken);
        return Ok(response);
    }

    /// <summary>Gets a single certification by id.</summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Trainer}")]
    [ProducesResponseType(typeof(ApiResponse<CertificationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCertification(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCertificationQuery { Id = id }, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error, statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(ApiResponse.Success(result.Value));
    }

    /// <summary>Creates a new certification.</summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCertification([FromBody] CreateCertificationRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCertificationCommand
        {
            ModuleId = request.ModuleId,
            Code = request.Code,
            Name = request.Name,
            Version = request.Version,
            Vendor = request.Vendor,
            Description = request.Description,
            DurationInMinutes = request.DurationInMinutes,
            PassingScore = request.PassingScore,
            CertificateValidityMonths = request.CertificateValidityMonths,
            DisplayOrder = request.DisplayOrder,
        };

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error, statusCode: StatusCodes.Status400BadRequest);
        }

        return CreatedAtAction(nameof(GetCertification), new { id = result.Value }, ApiResponse.Success(result.Value));
    }

    /// <summary>Updates an existing certification.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCertification(Guid id, [FromBody] UpdateCertificationRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCertificationCommand
        {
            Id = id,
            ModuleId = request.ModuleId,
            Code = request.Code,
            Name = request.Name,
            Version = request.Version,
            Vendor = request.Vendor,
            Description = request.Description,
            DurationInMinutes = request.DurationInMinutes,
            PassingScore = request.PassingScore,
            CertificateValidityMonths = request.CertificateValidityMonths,
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

    /// <summary>Soft-deletes a certification.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.SuperAdminOnly)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCertification(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteCertificationCommand { Id = id }, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error, statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(ApiResponse.Success<object?>(null));
    }
}
