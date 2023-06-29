using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.Common.Persistence.Uav;

public sealed record GetUavModelFromDbByIdQuery(Guid Id) : IRequest<ErrorOr<UavModel>>;