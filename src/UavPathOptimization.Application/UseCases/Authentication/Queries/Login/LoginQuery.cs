﻿using ErrorOr;
using MediatR;
using UavPathOptimization.Domain.Entities.Results;

namespace UavPathOptimization.Application.UseCases.Authentication.Queries.Login;

public sealed record LoginQuery(
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;