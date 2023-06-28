using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UavPathOptimization.Application.UseCases.UavModels.Commands.CreateUavModel;
using UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;
using UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModel;
using UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModelsPage;
using UavPathOptimization.Domain.Common.Enums;
using UavPathOptimization.Domain.Contracts.UavModel;
using UavPathOptimization.Domain.Entities.UavEntities;

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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUavModelsPage([FromQuery] GetUavModelsPageRequest request)
    {
        var query = new GetUavModelsPageQuery(
            request.Page,
            request.Size,
            request.Keyword,
            request.SortField,
            request.SortDirection
        );
        var result = await _mediator.Send(query);

        return result.Match<IActionResult>(
            success => Ok(success),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUavModel([FromRoute] Guid id, [FromBody] UpdateUavModelRequest request)
    {
        var command = new UpdateUavModelCommand(
            new UavModel
            {
                Id = id,
                Name = request.Name,
                MaxSpeed = request.MaxSpeed,
                MaxFlightTime = request.MaxFlightTime
            }
        );

        var result = await _mediator.Send(command);

        return result.Match<IActionResult>(
            success => StatusCode(StatusCodes.Status204NoContent),
            errors => Problem(errors)
        );
    }
}