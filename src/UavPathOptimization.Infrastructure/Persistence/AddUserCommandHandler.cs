using MediatR;
using UavPathOptimization.Application.Common.Persistence;
using UavPathOptimization.Infrastructure.Mappers;
using UavPathOptimization.Infrastructure.Persistence.EntityFramework;

namespace UavPathOptimization.Infrastructure.Persistence;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Unit>
{
    private readonly ApplicationDbContext _dbContext;

    public AddUserCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var mapper = new UserMapper();
        var infrastructureUser = mapper.UserToInfrastructureUser(request.User);

        _dbContext.Users.Add(infrastructureUser);
        _dbContext.SaveChanges();

        return Unit.Task;
    }
}