using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.Common.Persistence.Uav;

public sealed record AddUavModelToDbCommand(UavModel Uav) : IRequest<ErrorOr<Guid>>;