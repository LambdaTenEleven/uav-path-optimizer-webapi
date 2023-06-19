using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.Common.Persistence;

public record GetUserByEmailQuery(string Email) : IRequest<ErrorOr<User>>;