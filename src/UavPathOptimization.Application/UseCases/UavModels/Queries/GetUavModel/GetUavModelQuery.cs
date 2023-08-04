using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModel;

public sealed record GetUavModelQuery(Guid Id) : IRequest<ErrorOr<UavModel>>;