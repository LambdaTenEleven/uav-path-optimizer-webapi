using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.Common.Persistence.Uav;

public record AddUavModelToDbCommand(UavModel Uav) : IRequest<ErrorOr<Guid>>;