using MediatR;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.Common.Persistence;

public record CheckUserPasswordQuery(
    User User,
    string Password
) : IRequest<bool>;