using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.Common.Persistence.Uav;

public record AddUavModelToDbCommand(UavModel Uav) : IRequest<ErrorOr<Guid>>;