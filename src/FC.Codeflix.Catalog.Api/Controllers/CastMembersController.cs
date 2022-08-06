﻿using FC.Codeflix.Catalog.Api.ApiModels.Response;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.CreateCastMember;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.DeleteCastMember;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.GetCastMember;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FC.Codeflix.Catalog.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CastMembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CastMembersController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CastMemberModelOutput>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCastMemberInput input, 
        CancellationToken cancellationToken
    )
    {
        var output = await _mediator.Send(input, cancellationToken);
        return CreatedAtAction(
            nameof(GetById), 
            new { Id = output.Id },
            new ApiResponse<CastMemberModelOutput>(output)
        );
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CastMemberModelOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var output = await _mediator.Send(new GetCastMemberInput(id), cancellationToken);
        return Ok(new ApiResponse<CastMemberModelOutput>(output));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCastMemberInput(id), cancellationToken);
        return NoContent();
    }
}