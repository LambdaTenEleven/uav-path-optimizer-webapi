using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.Common.Persistence.Uav;

public record GetUavModelFromDbByNameQuery(string Name) : IRequest<ErrorOr<UavModel>>;