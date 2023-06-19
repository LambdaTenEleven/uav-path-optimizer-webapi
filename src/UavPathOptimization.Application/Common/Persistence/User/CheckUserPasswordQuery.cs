using MediatR;

namespace UavPathOptimization.Application.Common.Persistence.User;

public record CheckUserPasswordQuery(
    Domain.Entities.User User,
    string Password
) : IRequest<bool>;