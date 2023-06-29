using ErrorOr;
using MediatR;

namespace UavPathOptimization.Application.Common.Persistence.Uav;

public record DeleteUavModelFromDbCommand(Guid Id) : IRequest<ErrorOr<Unit>>;