using GeoCoordinatePortable;
using Riok.Mapperly.Abstractions;
using UavPathOptimization.Domain.Contracts;

namespace UavPathOptimization.Application.Mappers;

[Mapper]
internal partial class GeoCoordinateMapper
{
    public partial GeoCoordinateDto GeoCoordinateToGeoCoordinateDto(GeoCoordinate geoCoordinate);

    public partial GeoCoordinate GeoCoordinateDtoToGeoCoordinate(GeoCoordinateDto geoCoordinateDto);
}