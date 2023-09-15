using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UavPathOptimization.Application.UseCases.Schedule.Queries;
using UavPathOptimization.Domain.Contracts.Schedule;
using UavPathOptimization.WebAPI.Common;

namespace UavPathOptimization.WebAPI.Controllers;

[ApiController]
[Route("api/schedule")]
public class ScheduleController : ApiController
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public ScheduleController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UavScheduleResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] UavScheduleRequest request)
    {
        var query = _mapper.Map<CreateScheduleQuery>(request);
        var result = await _mediator.Send(query);

        return result.Match(
            ok => Ok(new UavScheduleResponse(result.Value.UavPathSchedules, result.Value.AbrasSchedule)),
                errors => Problem(errors)
        );
    }

}