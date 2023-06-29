using ErrorOr;
using MediatR;

namespace UavPathOptimization.Application.Common.Persistence.User;

public sealed record AddUserToDbCommand(Domain.Entities.User User, string Password) : IRequest<ErrorOr<Guid>>;