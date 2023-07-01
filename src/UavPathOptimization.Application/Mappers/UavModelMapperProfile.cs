// using AutoMapper;
// using UavPathOptimization.Application.UseCases.UavModels.Commands.CreateUavModel;
// using UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;
// using UavPathOptimization.Domain.Contracts.UavModel;
// using UavPathOptimization.Domain.Entities.UavEntities;
// using UnitsNet;
//

//
// public class UavModelMapperProfile : Profile
// {
//     public UavModelMapperProfile()
//     {
//         CreateMap<UavModel, UavModelResponse>()
//             .ForMember(x => x.MaxSpeed, opt => opt.MapFrom(x => x.MaxSpeed.KilometersPerHour));
//
//         CreateMap<CreateUavModelCommand, UavModel>()
//             .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
//             .ForMember(dest => dest.MaxSpeed, opt => opt.MapFrom(src => Speed.FromKilometersPerHour(src.MaxSpeed)));
//
//         CreateMap<UpdateUavModelCommand, UavModel>()
//             .ForMember(dest => dest.MaxSpeed, opt => opt.MapFrom(src => Speed.FromKilometersPerHour(src.MaxSpeed)));
//     }
// }

using Mapster;
using UavPathOptimization.Application.UseCases.UavModels.Commands.CreateUavModel;
using UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;
using UavPathOptimization.Domain.Contracts.UavModel;
using UavPathOptimization.Domain.Entities.UavEntities;
using UnitsNet;
using UnitsNet.Units;

namespace UavPathOptimization.Application.Mappers;

public class UavModelMapperProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UavModel, UavModelResponse>()
            .Map(dest => dest.MaxSpeed, src => src.MaxSpeed.KilometersPerHour);

        config.NewConfig<CreateUavModelCommand, UavModel>()
            .Map(dest => dest.MaxSpeed, src => new Speed(src.MaxSpeed, SpeedUnit.KilometerPerHour));

        config.NewConfig<UpdateUavModelCommand, UavModel>()
            .Map(dest => dest.MaxSpeed, src => new Speed(src.MaxSpeed, SpeedUnit.KilometerPerHour));
    }
}