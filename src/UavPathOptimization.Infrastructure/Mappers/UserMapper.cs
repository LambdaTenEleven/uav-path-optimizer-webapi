using Riok.Mapperly.Abstractions;
using UavPathOptimization.Domain.Entities;
using UavPathOptimization.Infrastructure.Persistence.EntityFramework;

namespace UavPathOptimization.Infrastructure.Mappers;

[Mapper]
internal partial class UserMapper
{
    public partial InfrastructureUser UserToInfrastructureUser(User user);

    public partial User InfrastructureUserToUser(InfrastructureUser infrastructureUser);
}