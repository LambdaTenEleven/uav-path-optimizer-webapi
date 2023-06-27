using ErrorOr;
using MediatR;

namespace UavPathOptimization.Application.UseCases.UavModels.Queries.GetUavModel;

public record GetUavModelQuery(Guid Id) : IRequest<ErrorOr<Domain.Entities.UavModel>>;