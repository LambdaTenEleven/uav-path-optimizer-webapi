using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UavPathOptimization.Application.Common.Persistence.User;
using UavPathOptimization.Infrastructure.Common.EntityFramework;

namespace UavPathOptimization.Infrastructure.Persistence.Authentication;

public class CheckUserPasswordQueryHandler : IRequestHandler<CheckUserPasswordQuery, bool>
{
    private readonly UserManager<InfrastructureUser> _userManager;

    private readonly IMapper _mapper;

    public CheckUserPasswordQueryHandler(UserManager<InfrastructureUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<bool> Handle(CheckUserPasswordQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.User.Id.ToString());
        if (user == null)
        {
            return false; //TODO change to error
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

        return isPasswordCorrect;
    }
}