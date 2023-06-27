using Mapster;
using UavPathOptimization.Domain.Contracts.Authentication;
using UavPathOptimization.Domain.Entities.Results;

namespace UavPathOptimization.Application.Mappers;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Token, src => src.Token)
            .Map(dest => dest, src => src.User);
    }
}