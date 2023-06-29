using ErrorOr;
using MediatR;

namespace UavPathOptimization.Application.Common.Persistence.User;

public sealed record GetUserByEmailQuery(string Email) : IRequest<ErrorOr<Domain.Entities.User>>;