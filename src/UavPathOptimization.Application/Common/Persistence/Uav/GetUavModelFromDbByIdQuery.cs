using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.Common.Persistence.Uav;

public record GetUavModelFromDbByIdQuery(Guid Id) : IRequest<ErrorOr<UavModel>>;