using MediatR;
using UavPathOptimization.Domain.Entities;

namespace UavPathOptimization.Application.Common.Persistence;

public record AddUserCommand(User User) : IRequest<Unit>;