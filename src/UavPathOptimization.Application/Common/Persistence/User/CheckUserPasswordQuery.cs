using MediatR;

namespace UavPathOptimization.Application.Common.Persistence.User;

public sealed record CheckUserPasswordQuery(
    Domain.Entities.User User,
    string Password
) : IRequest<bool>;