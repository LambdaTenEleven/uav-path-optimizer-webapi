using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UavPathOptimization.Application.UseCases.UavModel.Commands.CreateUavModel;
using UavPathOptimization.Application.UseCases.UavModel.Queries.GetUavModel;
using UavPathOptimization.Domain.Contracts.UavModel;

namespace UavPathOptimization.WebAPI.Controllers;

[Route("api/uav_model")]
public class UavModelController : ApiController
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public UavModelController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUavModel([FromBody] CreateUavModelRequest request)
    {
        var command = _mapper.Map<CreateUavModelCommand>(request);
        var result = await _mediator.Send(command);

        return result.Match<IActionResult>(
            success => StatusCode(StatusCodes.Status201Created),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUavModel([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new GetUavModelQuery(id));

        return result.Match<IActionResult>(
            success => Ok(success),
            errors => Problem(errors)
        );
    }
}