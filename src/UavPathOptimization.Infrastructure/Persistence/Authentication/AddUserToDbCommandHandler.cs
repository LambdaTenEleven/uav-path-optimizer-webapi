using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UavPathOptimization.Application.Common.Persistence.User;
using UavPathOptimization.Infrastructure.Common;
using UavPathOptimization.Infrastructure.Common.EntityFramework;

namespace UavPathOptimization.Infrastructure.Persistence.Authentication;

public class AddUserToDbCommandHandler : IRequestHandler<AddUserToDbCommand, ErrorOr<Guid>>
{
    private readonly UserManager<InfrastructureUser> _userManager;

    private readonly IMapper _mapper;
    public AddUserToDbCommandHandler(UserManager<InfrastructureUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Guid>> Handle(AddUserToDbCommand request, CancellationToken cancellationToken)
    {
        var infrastructureUser = _mapper.Map<InfrastructureUser>(request.User);

        var result = await _userManager.CreateAsync(infrastructureUser, request.Password);
        if (!result.Succeeded)
        {
            return result.Errors.Select(IdentityErrorConverter.Convert).ToList();
        }

        return infrastructureUser.Id;
    }
}