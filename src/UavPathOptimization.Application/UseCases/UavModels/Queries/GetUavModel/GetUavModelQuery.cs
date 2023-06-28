using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModel;

public record GetUavModelQuery(Guid Id) : IRequest<ErrorOr<UavModel>>;