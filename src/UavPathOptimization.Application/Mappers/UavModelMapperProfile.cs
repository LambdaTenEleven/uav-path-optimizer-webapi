using Mapster;
using UavPathOptimization.Application.UseCases.UavModels.Commands.CreateUavModel;
using UavPathOptimization.Application.UseCases.UavModels.Commands.UpdateUavModel;
using UavPathOptimization.Domain.Common;
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

        config.NewConfig<ResultPage<UavModel>, ResultPage<UavModelResponse>>()
            .ConstructUsing(src =>
                new ResultPage<UavModelResponse>(src.Items.Adapt<IEnumerable<UavModelResponse>>(), src.TotalCount, src.PageNumber,
                    src.PageSize));
    }
}