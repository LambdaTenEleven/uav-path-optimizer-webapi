using ErrorOr;
using MediatR;

namespace UavPathOptimization.Application.UseCases.UavModels.Commands.DeleteUavModel;

public sealed record DeleteUavModelCommand(Guid Id) : IRequest<ErrorOr<Unit>>;