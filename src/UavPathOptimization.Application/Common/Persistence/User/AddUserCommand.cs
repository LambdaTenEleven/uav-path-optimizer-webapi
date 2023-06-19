using ErrorOr;
using MediatR;

namespace UavPathOptimization.Application.Common.Persistence.User;

public record AddUserCommand(Domain.Entities.User User, string Password) : IRequest<ErrorOr<Guid>>;