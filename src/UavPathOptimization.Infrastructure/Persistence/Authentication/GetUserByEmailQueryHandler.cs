﻿using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UavPathOptimization.Application.Common.Persistence.User;
using UavPathOptimization.Domain.Common.Errors;
using UavPathOptimization.Domain.Entities;
using UavPathOptimization.Infrastructure.Common.EntityFramework;

namespace UavPathOptimization.Infrastructure.Persistence.Authentication;

public sealed class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, ErrorOr<User>>
{
    private readonly UserManager<InfrastructureUser> _userManager;

    private readonly IMapper _mapper;

    public GetUserByEmailQueryHandler(UserManager<InfrastructureUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ErrorOr<User>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return Errors.Login.UserNotFound;
        }

        return _mapper.Map<User>(user);
    }
}