using ErrorOr;
using MediatR;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.DeleteUavModel;

public record DeleteUavModelCommand(Guid id) : IRequest<ErrorOr<Unit>>;