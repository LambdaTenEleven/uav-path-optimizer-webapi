using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities.UavEntities;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;

public record UpdateUavModelCommand(UavModel UavModel) : IRequest<ErrorOr<Unit>>;