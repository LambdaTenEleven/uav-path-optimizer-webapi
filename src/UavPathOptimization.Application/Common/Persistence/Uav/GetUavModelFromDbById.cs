using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.Common.Persistence.Uav;

public record GetUavModelFromDbById(Guid Id) : IRequest<ErrorOr<UavModel>>;