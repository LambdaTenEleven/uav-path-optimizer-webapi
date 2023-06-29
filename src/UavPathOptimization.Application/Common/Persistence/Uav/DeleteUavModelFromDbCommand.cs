using ErrorOr;
using MediatR;

namespace UavPathOptimization.Application.Common.Persistence.Uav;

public sealed record DeleteUavModelFromDbCommand(Guid Id) : IRequest<ErrorOr<Unit>>;